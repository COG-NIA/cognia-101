# Cognia – User Documentation
**Prepared by:** Akubia Edith Elorm (Documentation and Report Writing - ID: 22188216)  
**Project:** Cognia (Student Mental Health Platform)  
**Date:** October 5, 2026 

---

## 1. Introduction

Cognia is a comprehensive **Mental Wellness Support System** designed to provide users with:

* Mental wellness tracking
* Professional therapy access
* Anonymous peer-to-peer community support

The system provides several features through one web application, including mood tracking, self-help resources, community discussions, therapist sessions, and anonymous support.

---

## 2. Getting Started

### 2.1 Accessing Cognia

Cognia is accessed through a web browser.

When running the application locally during development, the frontend is available at:

```text
http://localhost:5000
```

Users should open the application in a supported web browser.

---

### 2.2 Creating an Account

New users can create an account through the registration functionality.

1. Open **Cognia**.
2. Select **Register**.
3. Enter the required registration information.
4. Submit the registration form.
5. After successful registration, log in using the newly created account.

---

### 2.3 Logging In

1. Open **Cognia**.
2. Select **Login**.
3. Enter your username/email and password.
4. Select **Sign In**.

Cognia uses **ASP.NET Identity** together with **JWT-based authentication** to protect authenticated functionality.

---

### 2.4 Logging Out

To end an authenticated session:

1. Open the account menu.
2. Select **Logout**.
3. Confirm the logout if prompted.

---

## 3. Mood Tracker

The **Mood Tracker** allows users to record their daily emotional state and monitor changes in their wellbeing.

### 3.1 Recording a Mood

Users can record moods including:

* Happy
* Anxious
* Stressed
* Neutral

Users can also add an optional trigger note to provide additional context about their mood.

#### Steps

1. Open **Mood Tracker**.
2. Select your current mood.
3. Add an optional trigger note.
4. Submit the mood entry.

---

### 3.2 Viewing Mood Trends

Cognia provides **weekly and monthly mood trend charts**.

Users can use these charts to identify patterns in their emotional wellbeing and improve self-awareness.

---

## 4. Self-Help Hub

The **Self-Help Hub** provides curated mental wellness resources.

The feature is available without requiring users to log in.

### 4.1 Available Resources

Resources include:

* Mental wellness articles
* Breathing exercises
* Coping strategies

Content is categorized into areas such as:

* Stress
* Sleep
* Grief
* Academic pressure

---

### 4.2 Reading an Article

1. Open **Self-Help Hub**.
2. Browse the available categories.
3. Select an article.
4. Read the content and follow any recommended exercises or coping strategies.

---

## 5. Safe Space Forum

**Safe Space** is Cognia's anonymous peer-to-peer community.

Users can participate in discussions without publicly revealing their identity.

### 5.1 Forum Features

The forum supports:

* Discussion threads
* Replies
* Real-time communication
* Emoji reactions
* Trigger warnings
* Administrator content moderation

---

### 5.2 Viewing Discussions

1. Open **Safe Space**.
2. Browse available discussion threads.
3. Select a thread to view its replies.

---

### 5.3 Creating a Thread

1. Open **Safe Space**.
2. Select **Create Thread**.
3. Enter the topic and discussion content.
4. Add a trigger warning where appropriate.
5. Submit the thread.

---

### 5.4 Replying to a Thread

1. Open a discussion thread.
2. Enter your response.
3. Submit the reply.

Replies can be delivered in real time using Cognia's **SignalR** functionality.

---

### 5.5 Community Guidelines

Users should:

* Treat other community members respectfully.
* Avoid harassment and abusive language.
* Avoid sharing private information about other people.
* Use trigger warnings where appropriate.
* Avoid content that may put other users at risk.

Administrators are responsible for content moderation.

---

## 6. Therapist Sessions

Cognia allows users to browse verified therapist profiles and book paid one-on-one sessions.

### 6.1 Finding a Therapist

1. Open **Therapist Sessions**.
2. Browse available therapists.
3. Select a therapist.
4. Review the therapist's profile.
5. Select an available session.

---

### 6.2 Booking a Session

1. Select a therapist.
2. Select the desired session.
3. Proceed to payment.
4. Complete payment through **Paystack**.
5. After successful booking, access the provided **Jitsi** video-session link.

---

### 6.3 Payment

Cognia uses **Paystack** for therapist-session payments.

Supported payment methods include:

* MTN Mobile Money
* Bank/card payments where supported by Paystack

Users should confirm that their payment has been successfully processed before assuming that a session has been booked.

---

### 6.4 Video Sessions

Therapy sessions use **Jitsi** integration for online video communication.

Users should only share their session links with the intended participants.

---

## 7. Anonymous Support

The **Anonymous Support** feature allows users to submit help requests without publicly identifying themselves.

### Submitting a Request

1. Open **Anonymous Support**.
2. Enter the details of your concern.
3. Submit the request.
4. The request is routed to counsellors for appropriate attention.

Cognia also supports **daily check-in notifications** to encourage consistent wellbeing tracking.

---

## 8. Administrator Functionality

Administrators are responsible for managing aspects of the Cognia platform, including community content moderation.

The system includes a default administrator account after database seeding.

### Default Administrator Credentials

| Field    | Value              |
| -------- | ------------------ |
| Username | `admin@cognia.com` |
| Password | `Admin@123`        |

> **Important:** The default administrator password must be changed immediately after the first login.

---

## 9. User Safety

Cognia is a mental wellness support platform.

Users should seek appropriate **professional or emergency assistance** when experiencing serious or immediate mental health concerns.

Users should not use the community forum to share confidential information belonging to another person.

---

## 10. Troubleshooting

### I Cannot Log In

Check that:

* Your credentials are correct.
* Your account has been registered successfully.
* The Cognia backend is running.

---

### The Mood Tracker Is Not Loading

Check that:

* You are authenticated.
* The backend API is running.
* The database connection is working.

---

### Forum Messages Are Not Updating

Check that:

* The application is connected to the backend.
* SignalR is running correctly.
* Your network connection is active.

---

### Payment Is Not Completing

Check that:

* The payment information is correct.
* Paystack is available.
* The transaction has completed successfully.

---

For unresolved problems, report the issue through the project's GitHub repository.
