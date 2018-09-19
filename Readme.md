# POC of client disconnects during reading HttpContext.Request.Body

## How to run

1. Clone, compile and run
2. Use Postman to send a POST request to /home/index with a large request body (I used a 150 MB file)
    a) Body -> Binary -> Select file
3. Immediately cancel the request
4. Check c:\temp\log.txt that should contain "{current datetime}: Was cancelled"
    a) OR set a breakpoint on line 28 in HomeController.cs