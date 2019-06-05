FROM golang:1.9 as build
WORKDIR /src
COPY src/. .
RUN dir
RUN go get -d
RUN dir
RUN go build -a -o /app/app.exe .
COPY src/index.html /app/
RUN dir
WORKDIR /app
RUN dir

FROM mcr.microsoft.com/windows/nanoserver:1803
EXPOSE 8000
WORKDIR /app
RUN dir
COPY --from=build ./app .
RUN dir

ENTRYPOINT app.exe