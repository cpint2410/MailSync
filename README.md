# MailSync SaaS Platform

A modern, containerized productivity SaaS built with a **Modular Clean Architecture** pattern in .NET 11 and C#. 

## 🚀 Key Features & Architecture
- **Modular Monolith Design:** Separated into `MailSync.API`, `MailSync.Application`, `MailSync.Domain`, `MailSync.Infrastructure`, and `MailSync.Worker`.
- **Automated Database & Migrations:** Leverages **PostgreSQL 17** inside Docker with automated `DbContext.Database.Migrate()` execution on startup.
- **Background Ingestion Worker:** Asynchronous background service (`BackgroundService`) that polls connected user email accounts on regular intervals to ingest messages.
- **RESTful Endpoints:** Exposes high-performance endpoints (`/api/v1/mail/messages`) via ASP.NET Core Web API.
- **Full Docker Compose Stack:** Fully orchestrated multi-container deployment (`mailsync-db`, `mailsync-api`, `mailsync-worker`) with health checks and persistent volume mapping.
