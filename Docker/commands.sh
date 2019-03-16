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

#see docker ip
docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' containerId

#see container status
docker inspect -f '{{.State.Running}}' containerId

#see container details
docker inspect containerId

docker ls 
docker ls -a

Command	            #Description
docker attach	    #Attach local standard input, output, and error streams to a running container
docker build	    #Build an image from a Dockerfile
docker builder	    #Manage builds
docker checkpoint	#Manage checkpoints
docker commit	    #Create a new image from a container’s changes
docker config   	#Manage Docker configs
docker container	#Manage containers
docker cp	        #Copy files/folders between a container and the local filesystem
docker create	    #Create a new container
docker deploy	    #Deploy a new stack or update an existing stack
docker diff	        #Inspect changes to files or directories on a container’s filesystem
docker engine	    #Manage the docker engine
docker events	    #Get real time events from the server
docker exec	        #Run a command in a running container
docker export	    #Export a container’s filesystem as a tar archive
docker history	    #Show the history of an image
docker image	    #Manage images
docker images	    #List images
docker import	    #Import the contents from a tarball to create a filesystem image
docker info	        #Display system-wide information
docker inspect	    #Return low-level information on Docker objects
docker kill	        #Kill one or more running containers
docker load	        #Load an image from a tar archive or STDIN
docker login	    #Log in to a Docker registry
docker logout	    #Log out from a Docker registry
docker logs	        #Fetch the logs of a container
docker manifest	    #Manage Docker image manifests and manifest lists
docker network	    #Manage networks
docker node	        #Manage Swarm nodes
docker pause	    #Pause all processes within one or more containers
docker plugin	    #Manage plugins
docker port	        #List port mappings or a specific mapping for the container
docker ps	        #List containers
docker pull	        #Pull an image or a repository from a registry
docker push	        #Push an image or a repository to a registry
docker rename	    #Rename a container
docker restart	    #Restart one or more containers
docker rm	        #Remove one or more containers
docker rmi	        #Remove one or more images
docker run	        #Run a command in a new container
docker save	        #Save one or more images to a tar archive (streamed to STDOUT by default)
docker search	    #Search the Docker Hub for images
docker secret	    #Manage Docker secrets
docker service	    #Manage services
docker stack	    #Manage Docker stacks
docker start	    #Start one or more stopped containers
docker stats	    #Display a live stream of container(s) resource usage statistics
docker stop	        #Stop one or more running containers
docker swarm	    #Manage Swarm
docker system	    #Manage Docker
docker tag	        #Create a tag TARGET_IMAGE that refers to SOURCE_IMAGE
docker top	        #Display the running processes of a container
docker trust	    #Manage trust on Docker images
docker unpause	    #Unpause all processes within one or more containers
docker update	    #Update configuration of one or more containers
docker version	    #Show the Docker version information
docker volume	    #Manage volumes
docker wait	        #Block until one or more containers stop, then print their exit codes