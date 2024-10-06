# Employee Management System

## Overview

The Employee Management System is a web application designed to manage employee data. It allows users to perform CRUD operations on employee records, including creating, reading, updating, and deleting employee data. The application features real-time notifications, background processing, and a user-friendly interface.

## Technologies Used

- **Backend**: .NET Core Web API
- **Frontend**: Angular
- **Database**: SQL Server (Entity Framework Core)
- **Real-time Notifications**: SignalR
- **Background Job Processing**: Hangfire
- **UI Components**: Angular Material

## Features

### Backend

- **CRUD Operations**: Manage employee records.
- **Entity Framework Core**: For database operations and migrations.
- **SignalR Hub**: Sends real-time notifications on employee record changes.
- **Background Job**: Automatically deactivate employees who haven't logged in for the past 90 days.

### Frontend

- **Employee Grid**: Displays a list of employees with filtering, sorting, and pagination.
- **Employee Form**: For adding and editing employee information with validation.
- **Real-Time Notifications**: Toast messages for real-time updates on employee records.
- **Employee Status Highlighting**: Visually distinguish inactive employees in the grid.
- **Dashboard**: Summary of employee statistics (total, active, inactive).

## Bonus Features (Optional)

- **Role-Based Authorization**: Only admins can manage employee records.
- **Export Functionality**: Export employee list to CSV or Excel.
- **Unit Tests**: Coverage for critical application parts.

## Getting Started

### Prerequisites

- .NET Core SDK (version x.x.x)
- Node.js (version x.x.x)
- SQL Server (or any compatible database)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/AhmedYousry10/Employee-System.git
   cd Employee-System
   ```

### Developer Informations
- namme: **Ahmed Yousry Helal**
- email: **ahmedu3helal@gmail.com**
- phone: **+201007458070**
- linkedin: **https://www.linkedin.com/in/ahmed-yousry-helal/**
