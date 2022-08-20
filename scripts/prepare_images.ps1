# Script to build and tag docker images.
# Example usage:
# .\scripts\prepare_images.ps1 -serviceType computenode -tag v2 -push $True
param ($serviceType='frontend', $tag='v2', $push=$False)
Write-Host "Running the build and tag for $serviceType using tag $tag"

$ROOT_PATH = 'C:\Users\v-milkov\Documents\private\faks\master_rad\DistributedComputationSystem\'
cd $ROOT_PATH

$FRONTEND_NAME = 'frontend'
$COMPUTENODE_NAME = 'computenode'

# Frontend
if ($serviceType -eq $FRONTEND_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running build for $FRONTEND_NAME ..."
	docker build -f .\src\Frontend\Dockerfile --force-rm -t $FRONTEND_NAME ".\src"

	Write-Host "Running docker tag for $FRONTEND_NAME ..."
	docker tag frontend:latest matfmastercr.azurecr.io/frontend:$tag
	
	if ($push -eq $True)
	{
		Write-Host "Pushing image $FRONTEND_NAME to default repository ..."
		docker push matfmastercr.azurecr.io/frontend:$tag
	}
}


# ComputeNode
if ($serviceType -eq $COMPUTENODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running build for $COMPUTENODE_NAME ..."
	docker build -f .\src\ComputeNode\Dockerfile --force-rm -t computenode ".\src"

	Write-Host "Running docker tag for $COMPUTENODE_NAME ..."
	docker tag computenode:latest matfmastercr.azurecr.io/computenode:$tag
	
	if ($push -eq $True)
	{
		Write-Host "Pushing image $COMPUTENODE_NAME to default repository ..."
		docker push matfmastercr.azurecr.io/computenode:$tag
	}
}


