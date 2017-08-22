$ApplicationTypeName = "ECommerceType"
$ApplicationInstanceName = "fabric:/MyEcommerceApp"
$ApplicationVersion = "1.0.0"

# 1. Remove application instance
Remove-ServiceFabricApplication -ApplicationName $ApplicationInstanceName -Force

# 2. Unregister application type
Unregister-ServiceFabricApplicationType -ApplicationTypeName $ApplicationTypeName -ApplicationTypeVersion $ApplicationVersion -Force