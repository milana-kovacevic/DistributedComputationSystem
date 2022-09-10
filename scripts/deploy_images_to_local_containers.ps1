# Script  push docker images to AKS.
# Using predefined image repository.
# Example usage:
# .\scripts\deploy_images_to_local_containers.ps1 -serviceType frontend
param ($serviceType='frontend')


# TODO
# kreirati images
# ako postoji docker desktop container, azurirati ga
# pokrenuti docker conainer sa novim slikom