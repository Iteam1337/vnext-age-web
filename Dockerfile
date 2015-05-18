FROM tutum.co/iteamdev/aspnet-helloworld
ADD ./project.json /app/project.json
WORKDIR /app
RUN ["dnu", "restore"]

ADD . /app
EXPOSE 5004
CMD sleep 100000000000 | dnx . kestrel