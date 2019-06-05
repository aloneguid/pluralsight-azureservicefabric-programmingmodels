$Image = "aloneguid/psmandelgo:alpine"

docker build -f Dockerfile.linux -t $Image .

docker push $Image