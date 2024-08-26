A webapp that takes long URLs and shortens them. You can visit the deployed website [here](https://shorturls.danyalakt.com/).
Important note: If you want to actually shorten links, this website works best with very long links. Links that are around the
size of a youtube video link will stay around the same size. Part of the reason the links end up this way is because the domain
I'm hosting this website on has a long name (shorturls.danyalakt.com).

My approach with the backend was similar to the approach provided in this [video](https://www.youtube.com/watch?v=JQDHz72OA3c&ab_channel=TechDummiesNarendraL). I decided not to use Apache Zookeeper, and instead set up an RPC call with 
RabbitMQ (In hindsight, this was overkill, I could've setup a simple TCP listener instead, but I wanted to get more familiar with RabbitMQ at the time). This project was also my first taste of microservices.

I included .yaml files for Kubernetes in the ./backend. While this worked fine locally, setting up Kubernetes on the machine I deployed the website on was more trouble than it was worth, so I used docker compose for deployment instead.

Here's a simple diagram of the general architecture of the backend.


![https://imgur.com/wRfC3aX](https://imgur.com/wRfC3aX.png)


TODO: Links come with http, not https
etc...
