# Script to build and tag docker images.
# Example usage:
# .\scripts\prepare_images.ps1 -serviceType computenode -tag v2 -push $True
# .\scripts\prepare_images.ps1 -serviceType controlnode -tag v3.5 -push $True
param ($serviceType='controlnode', $tag='v2', $push=$False)
Write-Host "Running the build and tag for $serviceType using tag $tag"

$ROOT_PATH = 'C:\Users\v-milkov\Documents\private\faks\master_rad\DistributedComputationSystem\'
cd $ROOT_PATH

$CONTROLNODE_NAME = 'controlnode'
$COMPUTENODE_NAME = 'computenode'

# ControlNode
if ($serviceType -eq $CONTROLNODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running build for $CONTROLNODE_NAME ..."
	docker build -t controlnode -f .\src\ControlNode\Dockerfile --force-rm -t $CONTROLNODE_NAME ".\src"

	Write-Host "Running docker tag for $CONTROLNODE_NAME ..."
	docker tag controlnode:latest matfmastercr.azurecr.io/controlnode:$tag
	
	if ($push -eq $True)
	{
		Write-Host "Pushing image $CONTROLNODE_NAME to default repository ..."
		Write-Host "Image: matfmastercr.azurecr.io/controlnode:$tag"
		docker push matfmastercr.azurecr.io/controlnode:$tag
	}
}


# ComputeNode
if ($serviceType -eq $COMPUTENODE_NAME -or $serviceType -eq 'all')
{
	Write-Host "Running build for $COMPUTENODE_NAME ..."
	docker build -t computenode -f .\src\ComputeNode\Dockerfile --force-rm -t computenode ".\src"

	Write-Host "Running docker tag for $COMPUTENODE_NAME ..."
	docker tag computenode:latest matfmastercr.azurecr.io/computenode:$tag
	
	if ($push -eq $True)
	{
		Write-Host "Pushing image $COMPUTENODE_NAME to default repository ..."
		Write-Host "Image: matfmastercr.azurecr.io/computenode:$tag"
		docker push matfmastercr.azurecr.io/computenode:$tag
	}
}


