"# customer-info-app" 
# .NET MAUI Application Setup and Running Guide

This guide provides step-by-step instructions to open and run a .NET MAUI (Multi-platform App UI) application. Please follow the instructions carefully to ensure that the application runs smoothly.

## Prerequisites

Before running the app, you need to ensure that your development environment is properly set up.

### 1. Install Visual Studio 2022 or Later

Make sure you have Visual Studio 2022 or later installed on your system. You can download it from the official [Visual Studio website](https://visualstudio.microsoft.com/).

### 2. Install the .NET MAUI Workload

To work with .NET MAUI, you need to install the `.NET MAUI workload` during Visual Studio installation.

#### Steps:
- Launch the **Visual Studio Installer**.
- Choose **"Modify"** on your existing installation.
- Under **“Mobile development with .NET”**, ensure you check **“.NET MAUI”**.
- Click **Install** or **Update** as needed.

### 3. Install .NET 8 or .NET 7 SDK

Ensure that you have either **.NET 8** or **.NET 7 SDK** installed, depending on your app's target framework.

- You can download the latest version of .NET SDK from the official [Microsoft .NET download page](https://dotnet.microsoft.com/download).

### 4. Set Up Android Emulator or Physical Device for Testing

- For mobile testing, make sure you have an **Android Emulator** set up or a **physical Android/iOS device** connected.

---

## Step-by-Step to Open and Run a MAUI App

### Step 1: Open the Project

1. Launch **Visual Studio**.
2. Click **“Open a project or solution”**.
3. Navigate to your MAUI project folder and open the **`.sln`** (solution) file.

### Step 2: Set Startup Item to Server

1. In **Visual Studio**, select the **Startup Item**.
2. Set **Server** as the selected startup item.
3. Select **Default Project Server** for the correct project.

### Step 3: Open the Package Manager Console and Update Database

1. Open the **Package Manager Console** in Visual Studio.
   - Go to **Tools > NuGet Package Manager > Package Manager Console**.
2. Run the following command to update the database:

```bash
Update-Database
This will ensure your database schema is up to date with the latest changes.

Step 4: Set Multiple Startup Projects
Right-click on the .sln file in the Solution Explorer.

Select Properties.

In the properties window, select Multiple Startup Projects.

Set the appropriate actions for each project in the solution (e.g., Set "Start" for the necessary projects).

Step 5: Select Android Emulator and Run the Project
In the Startup Project dropdown at the top of Visual Studio, select Android Emulator (or the platform of your choice).

Click the Run button (green play button) to build and run your application.

Additional Notes
Emulator Setup: Ensure that you have configured the Android Emulator correctly through the AVD Manager in Visual Studio or Xcode (for iOS).

Device Testing: If you are using a physical Android/iOS device, ensure USB debugging is enabled and the device is connected to your computer.

Troubleshooting
If you face any issues, make sure:

Your SDK versions are compatible with your project.

Emulators are properly set up and configured.

Database connections are correctly set up and configured.

For more information on .NET MAUI and troubleshooting, refer to the official documentation:
Microsoft .NET MAUI Documentation
