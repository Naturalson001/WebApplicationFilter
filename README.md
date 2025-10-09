# WebApplicationFilter

A lightweight **ASP.NET Core middleware** and **rule-based filtering system** for detecting and blocking malicious requests such as SQL injection attempts, XSS payloads, or suspicious patterns.  

This project provides:
- A configurable **security rules engine**
- Logging of blocked/allowed requests
- Storage of logs and rules in **PostgreSQL**
- Easy integration into existing ASP.NET Core projects

---

## Features

- 🔒 Block or allow requests based on predefined rules  
- 📝 Log all suspicious activities into PostgreSQL (`security_logs` table)  
- ⚡ Hot-reload rules from the database (`security_rules` table)  
- 🌐 Works as middleware in any ASP.NET Core Web API  
- 📊 Exposes API endpoints to manage rules and logs  

---

## Tech Stack

- **.NET 9 / ASP.NET Core**
- **PostgreSQL** (with Docker support)
- **Entity Framework Core / Dapper** 
- **Swagger / OpenAPI** for API documentation

---

## Database Schema

```sql
CREATE TABLE security_rules (
    id TEXT PRIMARY KEY,
    is_blocked BOOLEAN NOT NULL,
    reason TEXT NOT NULL,
    threat_score INT NOT NULL,
    matched_patterns TEXT[] DEFAULT '{}'::text[] NOT NULL
);

CREATE TABLE security_logs (
    id SERIAL PRIMARY KEY,
    client_ip TEXT NOT NULL,
    endpoint TEXT NOT NULL,
    payload_snippet TEXT,
    threat_score INT,
    reason TEXT,
    matched_patterns TEXT[],
    was_blocked BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
