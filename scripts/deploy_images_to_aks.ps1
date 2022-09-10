# Script  push docker images to AKS.
# Using predefined image repository.
# Example usage:
# .\scripts\deploy_images_to_aks.ps1 -serviceType controlnode
param ($serviceType='controlnode', $namespace='distributed-system-dev-ns')

Write-Host "Running the deploy tp AKS for $serviceType "

$ROOT_PATH = 'C:\Users\v-milkov\Documents\private\faks\master_rad\DistributedComputationSystem\'
cd $ROOT_PATH

$CONTROLNODE_NAME = 'controlnode'
$COMPUTENODE_NAME = 'computenode'



# controlnode
if ($serviceType -eq $CONTROLNODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running update for $CONTROLNODE_NAME ..."
	kubectl apply --namespace=$namespace -f .\src\ControlNode\deploy-controlnode.yml
}



# ComputeNode
if ($serviceType -eq $COMPUTENODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running update for $COMPUTENODE_NAME ..."
	kubectl apply --namespace=$namespace -f .\src\ComputeNode\deploy-computenode.yml
}


