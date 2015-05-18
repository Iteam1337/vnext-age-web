FROM microsoft/aspnet
ADD . /app
WORKDIR /app
RUN ["dnu", "restore"]
EXPOSE 5004
CMD ["dnx", ".", "kestrel"]