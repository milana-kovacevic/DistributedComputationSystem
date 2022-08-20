# Script  push docker images to AKS.
# Using predefined image repository.
# Example usage:
# .\scripts\deploy_images_to_aks.ps1 -serviceType computenode

param ($serviceType='frontend', $namespace='distributed-system-dev-ns')
Write-Host "Running the build and tag for $serviceType "

$ROOT_PATH = 'C:\Users\v-milkov\Documents\private\faks\master_rad\DistributedComputationSystem\'
cd $ROOT_PATH

$FRONTEND_NAME = 'frontend'
$COMPUTENODE_NAME = 'computenode'



# Frontend
if ($serviceType -eq $FRONTEND_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running update for $FRONTEND_NAME ..."
	kubectl apply --namespace=$namespace -f .\src\Frontend\deploy-frontend.yml
}



# ComputeNode
if ($serviceType -eq $COMPUTENODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running update for $COMPUTENODE_NAME ..."
	kubectl apply --namespace=$namespace -f .\src\ComputeNode\deploy-computenode.yml
}


