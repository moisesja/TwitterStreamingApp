# Twitter Streaming App
This application is an evaluation challenge to join Jack Henry as one of their developers.

It makes use of the Twitter APIs V2 for a sample of tweets.

## Development Environment
### Dependencies
* .NET Core SDK 7.0
* Optional - Docker Desktop

### Setup
* Use a Git client, or the CLI to clone this repository
* Navigate to the local repository location on disk.
* Go to src/Console
* Edit the appsettings.json file and add the BearerToken value. Since this is a secret, please request it from the owner of this repo.
* Option 1:
    * Use the CLI under src/Console and run:
<!-- Bash script block -->
```bash
    dotnet run
```
* Option 2:
    * Use the CLI under src and run:
<!-- Bash script block -->
```bash
    docker build -t twitterstreamingappimg -f ./Console/Dockerfile .
``` 
* Once the Docker image is built. run: 

<!-- Bash script block -->
```bash
    docker run twitterstreamingappimg
``` 

---
**Disclosure. Input Data for Unit Tests has been directly taken from the random Twitter Feed. I'm not responsible for the contents.**
