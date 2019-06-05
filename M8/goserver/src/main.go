package main

import (
   "fmt"
   "io"
   "net/http"
   "runtime"
)

// https://thenewstack.io/building-a-web-server-in-go/

func hello(w http.ResponseWriter, r *http.Request) {

   var m runtime.MemStats
   runtime.ReadMemStats(&m)

   // PrintMemUsage outputs the current, total and OS memory being used. As well as the number
   // of garage collection cycles completed.
   io.WriteString(w,
      fmt.Sprintf("alloc = %v MiB, total alloc = %v MiB, sys = %v MiB, num gc = %v",
         bToMb(m.Alloc),
         bToMb(m.TotalAlloc),
         bToMb(m.Sys),
         m.NumGC))
}

func bToMb(b uint64) uint64 {
   return b / 1024 / 1024
}

func main() {
   fmt.Printf("server listening on port 8000")
	http.HandleFunc("/", hello)
	http.ListenAndServe(":8000", nil)
}
