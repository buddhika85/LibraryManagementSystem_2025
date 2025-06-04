# Library Management System
The Library Management System is a modern, scalable, and feature-rich demo application designed to streamline book cataloging, user management, borrowing transactions, and administrative operations. Built with a robust tech stack, the system ensures high performance, security, and an intuitive user experience.

## Key Features & Architecture

### Backend Development (Powered by ASP.NET Core & C#)
- Implements ASP.NET Identity for secure authentication and role-based access control.
- Uses Entity Framework Core (Code First) with SQL Server for seamless database management and optimized queries.
- Follows Specification & Repository Patterns to maintain a clean and modular data access layer.
  
### Frontend Development (Built with Angular & TypeScript)
- Designed with Angular for a dynamic and responsive UI, ensuring a smooth user experience.
- Utilizes Tailwind CSS & SCSS (@Mixins, built-in functions like lighten/darken, nested styles) for highly customizable and maintainable styling.
- Implements Angular Material for sleek UI components and consistency across the application.
  
### Continuous Integration & Deployment (CI/CD)
- GitHub Actions automates build, test, and deployment workflows.
- Azure Deployment ensures scalable hosting and optimized cloud performance.
  
### Testing & Logging
- NUnit for comprehensive unit testing, ensuring reliability and robustness.
- Integrated logging mechanisms for real-time monitoring and troubleshooting.


# Sprints

## Sprint 1: Project Setup and Basic Framework
- **Set up Git repository**
  - Initialize the repository
  - Create .gitignore file
  - Commit initial setup
- **Set up ASP.NET Core backend**
  - Create a new ASP.NET Core project
  - Install necessary Entity Framework Core packages
  - Create the database context
  - Configure the database connection
- **Set up Angular frontend**
  - Install Angular CLI
  - Create a new Angular project
  - Commit initial setup

## Sprint 2: User Authentication and Profile Management
- **Backend: User Authentication**
  - Implement member and staff registration endpoints
  - Implement login endpoints for members and staff
  - Set up authentication and session management
- **Frontend: User Authentication**
  - Create registration and login components
  - Implement forms for registration and login
  - Integrate authentication with the backend
- **Backend: Profile Management**
  - Implement endpoints for updating profile details
  - Implement password change functionality
- **Frontend: Profile Management**
  - Create profile management components
  - Implement forms for updating profile details
  - Integrate profile management with the backend

## Sprint 3: Book Management
- **Backend: Book Management**
  - Create endpoints for adding new books
  - Create endpoints for updating existing book details
  - Create endpoints for deleting books
  - Create endpoints for viewing and searching the book catalog
- **Frontend: Book Management**
  - Create book list component
  - Create add/edit book components
  - Create delete book functionality
  - Implement search functionality
  - Integrate book management with the backend

## Sprint 4: Borrowing and Returning Books
- **Backend: Borrowing Books**
  - Create endpoints for recording book borrowing transactions
  - Validate and store transaction information in the database
- **Backend: Returning Books**
  - Create endpoints for recording book return transactions
  - Update transaction information in the database
- **Frontend: Borrowing Books**
  - Create borrowing components for staff
  - Implement forms for recording borrowing transactions
  - Integrate borrowing functionality with the backend
- **Frontend: Returning Books**
  - Create returning components for staff
  - Implement forms for recording return transactions
  - Integrate return functionality with the backend
- **Backend & Frontend: Viewing Borrowing History**
  - Create endpoints for members to view their borrowing history
  - Implement history viewing components for members
  - Integrate borrowing history functionality with the backend

## Sprint 5: Member and Staff Management
- **Backend: Manage Members**
  - Create endpoints for editing member information
  - Create endpoints for removing members
- **Backend: Manage Staff**
  - Create endpoints for adding staff members
  - Create endpoints for removing staff members
- **Frontend: Manage Members**
  - Create member list component
  - Create edit member component
  - Implement remove member functionality
  - Integrate member management with the backend
- **Frontend: Manage Staff**
  - Create staff list component
  - Create add/edit staff components
  - Implement remove staff functionality
  - Integrate staff management with the backend

## Sprint 6: Testing and Deployment
- **Write unit tests for backend**
  - Ensure all endpoints are thoroughly tested
- **Write unit tests for frontend**
  - Ensure all components are thoroughly tested
- **Set up continuous integration and deployment (CI/CD)**
  - Automate testing processes
  - Automate deployment processes
- **Deploy the application**
  - Deploy the ASP.NET Core backend
  - Deploy the Angular frontend
  - Ensure both are seamlessly integrated

## Sprint 7: User Interface Enhancements (Optional)
- **Develop user-friendly interface**
  - Improve UI/UX based on feedback and testing
  - Implement responsive design
- **Enhance user experience**
  - Add additional features based on user needs
  - Continuously iterate based on feedback
