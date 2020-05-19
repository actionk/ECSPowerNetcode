# ECSPowerNetcode

The library is made on top of the (Unity Netcode)[https://docs.unity3d.com/Packages/com.unity.netcode@0.1/manual/index.html] package and saves you some time on configuring it / provides tools for client-server communication.

## Install

You can either just put the files into `Assets/Plugins/ECSEntityBuilder` or use it as a submodule:
```
git submodule add https://github.com/actionk/ECSPowerNetcode.git Assets/Plugins/ECSPowerNetcode
```

# Usage

## Starting a server and connecting to it locally

```
ServerManager.Instance.StartServer(7979);
ClientManager.Instance.ConnectToServer(7979);
```

After doing so, the library will automatically establish a connection and create command handlers for each connection in both client & server world.

## Accessing connection's entities

Each connection is described by these parameters:

```
public struct ConnectionDescription
{
    public int networkId; // unique network id value for client-server connection
    public Entity connectionEntity;
    public Entity commandHandlerEntity;
}
```

### Client side

`ClientManager.Instance.IsConnected`

`ClientManager.Instance.ConnectionToServer` -> `ConnectionDescription` struct

### Server side

`ServerManager.Instance.AllConnections` - getting list of all connected clients of `ConnectionDescription` struct

`ServerManager.Instance.GetClientConnectionByNetworkId`

## Groups

### Client-side

```
ClientConnectionSystemGroup
ClientRequestProcessingSystemGroup
ClientNetworkEntitySystemGroup
ClientGameSimulationSystemGroup
```

| Group | Description |
| --- | --- |
| ClientConnectionSystemGroup | Connection/Disconnection from server |
| ClientRequestProcessingSystemGroup | Processing requests from server |
| ClientNetworkEntitySystemGroup | Processing network entities |
| ClientGameSimulationSystemGroup | All your game simulations on client side |


### Server-side

```
ServerConnectionSystemGroup
ServerRequestProcessingSystemGroup
ServerNetworkEntitySystemGroup
ServerGameSimulationSystemGroup
```

| Group | Description |
| --- | --- |
| ServerConnectionSystemGroup | Connection/Disconnection from clients |
| ServerRequestProcessingSystemGroup | Processing requests from clients |
| ServerNetworkEntitySystemGroup | Processing network entities |
| ServerGameSimulationSystemGroup | All your game simulations on server side |

## Command builders

For making your life easier, there are command builders for both client & server commands.
First of all, you have to create an IRpcCommand yourserlf
