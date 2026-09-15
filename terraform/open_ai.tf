resource "azurerm_cognitive_account" "open_ai" {
  name                  = local.open_ai_config_name
  location              = azurerm_resource_group.this.location
  resource_group_name   = azurerm_resource_group.this.name
  kind                  = "OpenAI"
  sku_name              = "S0"
  tags                  = local.tags
  custom_subdomain_name = local.open_ai_config_name

  identity {
    type = "SystemAssigned"
  }
}

/*
resource "azurerm_cognitive_deployment" "gpt_5_mini" {
  name                 = "gpt-5-mini"
  cognitive_account_id = azurerm_cognitive_account.open_ai.id
  model {
    format  = "OpenAI"
    name    = "gpt-5-mini"
    version = "2025-08-07"
  }

  sku {
    name     = "GlobalStandard"
    capacity = 10
  }
}
*/