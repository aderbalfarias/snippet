# Note: Your machine needs to have Azure Function Core Tools running 
#       in order to create azure function from command line
npm install -g azure-functions-core-tools

# Creating a function using powershell
func
func init # Select a worker runtime and language 
func start # Start a function

# More about bindings
# https://docs.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings