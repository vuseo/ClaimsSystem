# ClaimsSystem

# This is a simple claims system implemented in Python. It allows users to create, view, and manage insurance claims. 
# The system uses swagger for interaction and is designed to be easily deployable to Azure.

# CURRENT:

# Client
# ↓
# ASP.NET Core API
# ↓
# Azure Service Bus (Queue)
# ↓
# Background Worker
# ↓
# Local SQL Database

# For a production level implementation, you would typically use a cloud-based database and message queue service,
# such as Azure SQL Database and Azure Service Bus. 
# The claims worker would be responsible for processing messages from the queue and updating the database accordingly.

# PRODUCTION:

# Client
# ↓
# Azure API Management
# ↓
# ASP.NET Core API (Azure App Service)
# ↓
# Azure Service Bus (Queue)
# ↓
# Azure Function (Worker)
# ↓
# Azure SQL Database
