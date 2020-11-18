# .NET Azure IoT Hubs Device-Twins Demo

A .NET Core Azure IoT Hubs Device Twins Demo App

# Instructions

- Create a new Azure IoT Hub in the Standard or Free Tier.
- Create an IoT Device.
- Add a Desired Property of;

```JSON
"fanSpeed": 10,
```

- Run the application with;

```cs
dotnet run
```

- Copy the Primary Key from the IoT Hub registryRead Shared Access Policy
- Paste into the console when prompted
- Copy the Primary Connection String from the IoT Device
- Paste into the console when prompted
- Copy the Device ID from the Device Details Page
- Paste into the conosle when prompted
- Change the `fanspeed` Desired Property value in the Device Twin and hit the `Save` button.
- View the change in the console.
- Enter a value for the Cooler Control.
- Refresh the Device Twin and view the `coolerOn` Reported Property.