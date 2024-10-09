docker build -t aspnetapp -f .\BuildAndDeploy\dockerfile .

az acr build -r rbrcr -t rbrcr.azurecr.io/aspnetapp:latest --platform linux .
