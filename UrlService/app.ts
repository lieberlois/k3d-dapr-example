import { DaprServer, HttpMethod } from "dapr-client";
import { UrlResponse } from "./models/urlResponse";
import { UrlRequest } from "./models/urlRequest";

const baseUrl = "http://www.example.com/";

async function start() {
  const server = new DaprServer();

  // Note that invoker listeners can be set up after start() has been called
  await server.start();

  console.log("Setting up invocation endpoints...")

  await server.invoker.listen("url", async (data: Record<string, any>): Promise<UrlResponse> => {
    // Data is automatically parsed when received
    console.log(`Received: ${data.body} on POST url`);

    const { title }: UrlRequest = JSON.parse(data.body);

    const resp: UrlResponse = {
        url: baseUrl + title.toLowerCase().replace(/ /g, "-")
    }

    return resp;
  }, { method: HttpMethod.POST });
}

start().catch((e) => {
  console.error(e);
});
