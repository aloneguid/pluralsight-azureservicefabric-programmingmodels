FROM golang:1.9 as build
WORKDIR /src
COPY src/. .
RUN go get -d
RUN go build -a -o /app/app .
COPY src/index.html /app/
WORKDIR /app

FROM alpine:3.7
EXPOSE 8000
WORKDIR /app
COPY --from=build ./app .

ENTRYPOINT app