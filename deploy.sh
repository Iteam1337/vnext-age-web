docker build -t faceproxy .
docker tag -f faceproxy tutum.co/iteamdev/aspnet-faceproxy
docker push tutum.co/iteamdev/aspnet-faceproxy