terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.0.0"
    }
  }
}

# Microsoft Azure Provider
provider "azurerm" {
  features {}
}

# Azure

resource "azurerm_resource_group" "k3d" {
  name     = "k3d-dapr-example"
  location = "West Europe"
}

resource "azurerm_servicebus_namespace" "k3d" {
  name                = "k3d-dapr-example"
  location            = azurerm_resource_group.k3d.location
  resource_group_name = azurerm_resource_group.k3d.name
  sku                 = "Standard"

  tags = {
    source = "terraform"
  }
}

# # Secrets for Kubernetes Cluster

output "service_bus_conn" {
  description = "Connection string for Azure Service Bus Namespace"
  value       = azurerm_servicebus_namespace.k3d.default_primary_connection_string
  sensitive   = true
}