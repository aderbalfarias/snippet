docker run
    -d #(run detached) 
    -it #(interactive terminal)
    -p #(publish a port)
    -v #(mount a volume)
    --name #(name a container)
    --link #(communicate between containers)
    --rm #(auto-delete when container stops)
    -e NAME=value #(environment variables)
docker exec 
    #Run a command inside a container
docker ps 
    #See what containers are running
docker logs 
    #View log output from a container
Clean up
    docker rm –f container-name 
    docker image rm image-name
    docker volume rm volume-name
