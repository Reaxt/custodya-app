# Design Document - Custodya

Rea Koehler, Liam Kopke & Kevin-Christopher Laskai

## Design Overview

Our application is a user interface for the azure connected container farms our company is renting out. The app allows farmers and fleet owners to remotely monitor and control the sensors and actuators inside the container as well as receive alerts and notifications about any changes or issues. The tool also allows users to share graphs, reports, results or events with interested parties.

## App Prototype

Describe how the app will function in greater detail. Include the description of each screen, as well as screen design, and any information about the navigation between the screens.

Our app will first welcome the user to login. Using their credentials, the user will then land on our home page. From there, they can navigate through the app via the navigation icons at the bottom of the screen. 

As a general overview, here is the screen relations diagram. Each page is described in depth below.

![Screen Relations Diagram](/src/images/screen-relations.png)

### Pages

#### Login

![Login Page](/src/images/login_page.jpg)

The login screen is the first page that users will see when they open the application. It allows them to log in using their credentials. Once properly authenticated, users will then be navigated to the [home page](#Home).

#### Navigation

![Navigation bar](/src/images/nav.jpg)

The navigation bar at the bottom of the screen allows users to navigate to the [home page](#home), the [tasks page](#tasks), the [notifications page](#notifications) and the [tools page](#tools). This same navigation bar is accessible on all pages other than the [login page](#login).

#### Account

![Account](/src/images/account.jpg)

By selecting the account icon at the top right of any page, the user will be redirected to the account page. On this page, the user will be able to see and edit their account information and credentials.

#### Home

![Home](/src/images/home.jpg)

Once on the home page, users can then navigate to another page, select a pod and/or see their tasks for the day. By selecting a pod, the user is then redirected to the [pod dashboard page](#dashboard). Selecting the task view will redirect the user to [task page](#tasks).

#### Pod

##### Dashboard

![Pod Dashboard](/src/images/pod_dashboard.jpg)

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

![Pod Plants Pop up 1](/src/images/pod_plants_popup1.jpg)

The first pop up allows the user to select whether they want auto or manual. Selecting manual on a sensor will then display the second pop up.

###### Pop up 2

![Pod Plants Pop up 2](/src/images/pod_plants_popup2.jpg)

The second pop up allows the user to enter specific ranges for notifications for that sensor.

##### Plants

![Pod Plants](/src/images/pod_plants.jpg)

In the top section, the plants page displays readings from the temperature, humidity and soil moisture sensors while the bottom section allows the user to control those sensors and actuators for a fan and a light.

##### Geolocation

In the top section, the geolocation page displays readings from the GPS location, pitch angle / roll angle and vibration level sensors while the bottom section allows the user to control those sensors and actuators for a buzzer.

##### Security

In the top section, the security page displays readings from the noise, luminosity, motion sensors while the bottom section allows the user to control those sensors and actuators for a servo door lock and a buzzer.

#### Tasks

![Tasks](/src/images/tasks.jpg)

The tasks page allows user to see their tasks separated by pod and by status. These slicers are situated at the top. The main body is a list view of each task. These tasks can be edited and deleted. Selecting edit on a task will redirect the user to the [edit task page](#edit). A new task can be added by selecting the add button at the bottom right. This navigates the user to the [add task page](#add).



Both the [add](#add) and [edit](#edit) pages are identical in appearance except for the different titles and save button. The fields in these screen are a title, a date time that can be changed via a date picker pop up, a recurrence and a pod that task is attributed to. 

##### Add

![Tasks add](/src/images/tasks_add.jpg)

The add page starts with an empty task object

##### Edit

![Tasks edit](/src/images/tasks_edit.jpg)

The edit page is passed a task object and edits it.

#### Notifications

![Notifications](/src/images/notifications.jpg)

The notifications page, much like the [tasks page ](#tasks) has slicers at the top to separate its list content. Each notification can be deleted.





## App Features

Create a list of all features that the app should achieve. Include all the ideas you have discussed with your teammates even the ones you do not think they will be developed due to lack of time.

- Organize the features into epic user stories.
- Then create small (testable) user stories of all the features you have created.
- Prioritization of Features:

the user stories (features) must be categorized by priority as:

**Must develop**

Farm Technician:

- As a farm technician, I want to be able to read temperature values so that I can be informed.
- As a farm technician, I want to be able to read humidity values so that I can be informed.
- As a farm technician, I want to be able to read ground moisture values so that I can be informed.
- As a farm technician, I want to be able to control my fan from the app so that I can control the temperature.As a farm technician, I want to be able to control my light from the app to control the luminosity.
- As a farm technician, I want to be able to read my fan state from the app so that I can make any changes.
- As a farm technician, I want to be able to read my light state from the app so that I can make any changes.
- As a farm technician, I want to be able to set whether actuators are automatically set or manually set when a limit is reached so that I can add the most amount of customization to the app.
- As a farm technician, I want to be able to log in to my account and see all of my information so that my experience is personalized and isn’t the same regardless of the account.
- As a farm technician, I want to have all my sensor values be displayed in a main dashboard so that I can easily see if my operations are running smoothly.
- As a farm technician, I want to be able to see each section of my pods (plants, security, geo-location) on their own dashboard so that I can see more in-depth information about each.

Fleet Owner

- As a fleet owner, I want to see the exact location of the container via the GPS location tracker in a neatly displayed format so that it’s easy-to-read.
- As a fleet owner wants to get notified if the pitch and roll angle reach a certain amount during transportation to know if the transport company made any errors.
- As a fleet owner I want to get notified if the pitch and roll angle changes after installation to see if there is any issue with the land that it was installed on for a quick response.
- As a fleet owner I want to get live pitch and roll angles displayed on the app to see if there are any potential issues.
- As a fleet owner I want to get live vibrations levels displayed in the app in case of potential issues
- As the fleet owner I want to get notified if the vibration levels get to a certain amount after installation of the container in case that there is an event happening while not looking at the app
- As the fleet owner I want to get notified if the vibration levels get to a certain amount during transportation of the container to make sure that there wasn’t any incidence.
- As a fleet owner I want to see the state of the buzzer through the app in case someone left it on or off at the wrong times
- As a fleet owner I want to control the buzzer’s state through the app to turn the buzzer off or on incase someone left it on or off
- As a fleet owner I want all features mentioned to be only accessible to fleet owners to ensure the security of the container
- As a fleet owner I want to be able to check if the doors of the container are opened or closed to ensure expected behavior
- As a fleet owner I want to be able to check if the doors of a container are locked to ensure security of the container
- As a fleet owner I want to be notified whenever a door is opened in the container to ensure nothing unexpected happens
- As a fleet owner I want to be able to set a doors locked state in the container to ensure no unauthorized users obtain access
  **Would like to develop**

- As a farm technician, I want to be able to receive notifications about limits I set on my sensors so that I can then use the correct actuators to resolve the situation.
- As a farm technician, I want to receive push notifications to my device whenever any notification occurs, so I can be alerted of any behaviors that are unexpected
- As a farm technician, I want to be able to set limits on my sensor values so that I get notifications when the values exceed certain thresholds.
- As a farm technician, I want to be able to have all my pods accessible on the same app so that I can have different growing conditions in each without having to make a new account for each pod. As a farm technician, I want to be able to delete notifications so that they don’t clutter

**Could develop if time permits**

- As a farm technician, I want to be able to reset my password in case I forget it so that I don’t get locked out of my account.
- As a farm technician, I want to be able to set tasks in the app to act as reminders.
- As a farm technician, I want to be able to sort my tasks by the pod they are for so that I can see what needs to be done where.As a farm technician, I want to be able to sort my tasks by status (today, upcoming & completed) so that I can better plan out my days.
- As a farm technician, I want to be able to delete a task so that they don’t clutter.
- As a farm technician, I want to be able to edit a task in case I made a mistake.
- As a farm technician, I want to be able to organize my notifications by the pod they came from so that I can know what incident happened where.As a farm technician, I want to be able to edit the setting (color, name) of each of my pods so that I can differentiate them easily.

### Likely would not develop because of lack time or knowledge

## Potential Showstoppers and Open Questions

We are not very motivated to make a big final project.

Will we have enough time?