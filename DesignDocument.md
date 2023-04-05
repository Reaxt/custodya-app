# Design Document - Custodya

Rea Koehler, Liam Kopke & Kevin-Christopher Laskai

## Design Overview

Our application is a user interface for the azure connected container farms our company is renting out. The app allows farmers and fleet owners to remotely monitor and control the sensors and actuators inside the container as well as receive alerts and notifications about any changes or issues. The tool also allows users to share graphs, reports, results or events with interested parties.

## App Prototype

Describe how the app will function in greater detail. Include the description of each screen, as well as screen design, and any information about the navigation between the screens.

Our app will first welcome the user to login. Using their credentials, the user will then land on our home page. From there, they can navigate through the app via the navigation icons at the bottom of the screen. 

As a general overview, here is the screen relations diagram. Each page is described in depth below.

![Screen Relations Diagram](.\src\images\screen-relations.png)

### Pages

#### Login

![Login Page](.\src\images\login_page.jpg)

The login screen is the first page that users will see when they open the application. It allows them to log in using their credentials. Once properly authenticated, users will then be navigated to the [home page](#Home).

#### Navigation

![Navigation bar](.\src\images\nav.jpg)

The navigation bar at the bottom of the screen allows users to navigate to the [home page](#home), the [tasks page](#tasks), the [notifications page](#notifications) and the [tools page](#tools). This same navigation bar is accessible on all pages other than the [login page](#login).

#### Account

![Account](.\src\images\account.jpg)

By selecting the account icon at the top right of any page, the user will be redirected to the account page. On this page, the user will be able to see and edit their account information and credentials.

#### Home

![Home](.\src\images\home.jpg)

Once on the home page, users can then navigate to another page, select a pod and/or see their tasks for the day. By selecting a pod, the user is then redirected to the [pod dashboard page](#dashboard). Selecting the task view will redirect the user to [task page](#tasks).

#### Pod

##### Dashboard

![Pod Dashboard](.\src\images\pod_dashboard.jpg)

The pod dashboard page displays all sensor readings for the three categories of the app ([plants](#plants), geo-locations & security). This allows the user to get a quick overview of their pod before accessing a specific category. These categories can be accessed by selecting the appropriate tab at the top of the screen or by selecting the respective section in the body of the page. Selecting a category will redirect the user to the appropriate page. 

All three of those other pages are very similar in design, so only the [plants page](#plants) will have a wireframe. The three sections are designed as follows:

The top section of the page displays sensor readings for that section.

The bottom page on the other hand allows the user to control the sensors and the actuators. Both sensors and actuators can be configured to manual or auto and on or off.

Sensors:

- Manual: User sets range of values for sensors. If the sensor reads outside of that range, a notification will be sent.
- Auto: A very complex AI (hard coded values) decides the range.
- On/Off: Whether to send a notification about values outside of range.

Actuators:

- Manual: The user has to manually turn on the actuator
- Auto: The app auto enables and shuts off actuators to keep the values within the sensor range.
- On/Off: Whether to send a notification whenever the actuator is turned on/off.

The manual and auto values are controlled by pop ups.

###### Pop up 1

![Pod Plants Pop up 1](.\src\images\pod_plants_popup1.jpg)

The first pop up allows the user to select whether they want auto or manual. Selecting manual on a sensor will then display the second pop up.

###### Pop up 2

![Pod Plants Pop up 2](.\src\images\pod_plants_popup2.jpg)

The second pop up allows the user to enter specific ranges for notifications for that sensor.

##### Plants

![Pod Plants](.\src\images\pod_plants.jpg)

In the top section, the plants page displays readings from the temperature, humidity and soil moisture sensors while the bottom section allows the user to control those sensors and actuators for a fan and a light.

##### Geolocation

In the top section, the geolocation page displays readings from the GPS location, pitch angle / roll angle and vibration level sensors while the bottom section allows the user to control those sensors and actuators for a buzzer.

##### Security

In the top section, the security page displays readings from the noise, luminosity, motion sensors while the bottom section allows the user to control those sensors and actuators for a servo door lock and a buzzer.

#### Tasks

![Tasks](.\src\images\tasks.jpg)

The tasks page allows user to see their tasks separated by pod and by status. These slicers are situated at the top. The main body is a list view of each task. These tasks can be edited and deleted. Selecting edit on a task will redirect the user to the [edit task page](#edit). A new task can be added by selecting the add button at the bottom right. This navigates the user to the [add task page](#add).



Both the [add](#add) and [edit](#edit) pages are identical in appearance except for the different titles and save button. The fields in these screen are a title, a date time that can be changed via a date picker pop up, a recurrence and a pod that task is attributed to. 

##### Add

![Tasks add](.\src\images\tasks_add.jpg)

The add page starts with an empty task object

##### Edit

![Tasks edit](.\src\images\tasks_edit.jpg)

The edit page is passed a task object and edits it.

#### Notifications

![Notifications](.\src\images\notifications.jpg)

The notifications page, much like the [tasks page ](#tasks) has slicers at the top to separate its list content. Each notification can be deleted.





## App Features

Create a list of all features that the app should achieve. Include all the ideas you have discussed with your teammates even the ones you do not think they will be developed due to lack of time.

- Organize the features into epic user stories.
- Then create small (testable) user stories of all the features you have created.
- Prioritization of Features:

the user stories (features) must be categorized by priority as:

### Must develop

### Would like to develop

### Could develop if time permits

### Likely would not develop because of lack time or knowledge

## Potential Showstoppers and Open Questions

We are not very motivated to make a big final project.

Will we have enough time?

How much wood could a woodchuck chuck if a woodchuck could chuck wood?