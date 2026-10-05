# Cognia API Documentation
**Prepared by:** Akubia Edith Elorm (Documentation and Report Writing - ID: 22188216)  
**Project:** Cognia (Student Mental Health Platform)  
**Date:** October 5, 2026 

---

## 1. Overview

Cognia provides a RESTful backend API implemented using **ASP.NET Core Web API on .NET 10.0**.

The API supports:

* Authentication
* Mood tracking
* Self-help resources
* Community forums
* Therapist sessions
* Payment processing

---

## 2. Base URL

During local development, the backend API runs at:

```text
https://localhost:5001
```

Therefore, an endpoint such as:

```http
POST /api/auth/login
```

is accessed locally through the API server.

---

## 3. Authentication

Cognia uses **ASP.NET Identity** with **JWT authentication**.

### 3.1 Register User

**Endpoint**

```http
POST /api/auth/register
```

**Purpose:**
Creates a new Cognia user account.

**Method:** `POST`

**Authentication:** Not required for registration.

---

### 3.2 Login

**Endpoint**

```http
POST /api/auth/login
```

**Purpose:**
Authenticates a registered user.

**Method:** `POST`

**Authentication:** Not required.

The login process uses JWT-based authentication.

---

### 3.3 Logout

**Endpoint**

```http
POST /api/auth/logout
```

**Purpose:**
Logs the current user out of the application.

**Method:** `POST`

---

## 4. Mood Tracker API

### 4.1 Get Mood Entries

**Endpoint**

```http
GET /api/mood
```

**Purpose:**
Retrieves the authenticated user's mood entries.

**Method:** `GET`

**Authentication:** Required.

---

### 4.2 Create Mood Entry

**Endpoint**

```http
POST /api/mood
```

**Purpose:**
Records a new mood entry.

**Method:** `POST`

**Authentication:** Required.

The supported mood types documented by the application include:

* Happy
* Anxious
* Stressed
* Neutral

An optional trigger note may also be included.

---

### 4.3 Get Mood Trends

**Endpoint**

```http
GET /api/mood/trends
```

**Purpose:**
Retrieves mood trend information used by the mood-tracking dashboard.

**Method:** `GET`

**Authentication:** Required.

The frontend uses this information to display weekly and monthly mood trends.

---

## 5. Self-Help Hub API

### 5.1 Get All Articles

**Endpoint**

```http
GET /api/articles
```

**Purpose:**
Retrieves available mental wellness articles.

**Method:** `GET`

**Authentication:** Not required according to the documented accessibility of the Self-Help Hub.

---

### 5.2 Get Article

**Endpoint**

```http
GET /api/articles/{id}
```

**Purpose:**
Retrieves a specific article using its identifier.

**Method:** `GET`

**Path Parameter:**

| Parameter | Description               |
| --------- | ------------------------- |
| `id`      | Identifier of the article |

---

### 5.3 Get Article Categories

**Endpoint**

```http
GET /api/articles/categories
```

**Purpose:**
Retrieves the available article categories.

**Method:** `GET`

Documented categories include:

* Stress
* Sleep
* Grief
* Academic pressure

---

## 6. Forum API

### 6.1 Get All Threads

**Endpoint**

```http
GET /api/forum/threads
```

**Purpose:**
Retrieves forum discussion threads.

**Method:** `GET`

---

### 6.2 Create Thread

**Endpoint**

```http
POST /api/forum/threads
```

**Purpose:**
Creates a new forum discussion thread.

**Method:** `POST`

**Authentication:** Required.

The forum supports anonymous peer-to-peer community discussions.

---

### 6.3 Get Thread Replies

**Endpoint**

```http
GET /api/forum/threads/{id}/replies
```

**Purpose:**
Retrieves replies associated with a forum thread.

**Method:** `GET`

**Path Parameter:**

| Parameter | Description                    |
| --------- | ------------------------------ |
| `id`      | Identifier of the forum thread |

---

### 6.4 Add Thread Reply

**Endpoint**

```http
POST /api/forum/threads/{id}/replies
```

**Purpose:**
Adds a reply to an existing forum thread.

**Method:** `POST`

**Path Parameter:**

| Parameter | Description                    |
| --------- | ------------------------------ |
| `id`      | Identifier of the forum thread |

The forum also supports real-time communication through **SignalR**.

---

## 7. Therapist API

### 7.1 Get Therapists

**Endpoint**

```http
GET /api/therapists
```

**Purpose:**
Retrieves available verified therapist profiles.

**Method:** `GET`

---

### 7.2 Get Therapist Profile

**Endpoint**

```http
GET /api/therapists/{id}
```

**Purpose:**
Retrieves the profile of a specific therapist.

**Method:** `GET`

**Path Parameter:**

| Parameter | Description                 |
| --------- | --------------------------- |
| `id`      | Identifier of the therapist |

---

## 8. Session API

### 8.1 Book Session

**Endpoint**

```http
POST /api/sessions
```

**Purpose:**
Creates a therapist-session booking.

**Method:** `POST`

The booking process is associated with the therapist-session functionality and payment workflow.

---

## 9. Payment API

### 9.1 Process Payment

**Endpoint**

```http
POST /api/payments
```

**Purpose:**
Processes payment associated with a therapist session.

**Method:** `POST`

Cognia integrates with the **Paystack API** for payment processing.

Supported payment options documented by the project include:

* MTN Mobile Money
* Card payments

---

## 10. Real-Time Communication

Cognia uses **SignalR** for real-time forum functionality.

The project contains a dedicated:

```text
Cognia.API/Hubs/
```

directory for SignalR hubs.

The real-time system supports features such as:

* Real-time forum replies
* Real-time communication between connected clients

---

## 11. HTTP Status Codes

The following standard HTTP status codes may be used by the API:

| Status Code | Meaning                       |
| ----------- | ----------------------------- |
| `200`       | Successful request            |
| `201`       | Resource successfully created |
| `400`       | Bad request                   |
| `401`       | Unauthorized                  |
| `403`       | Forbidden                     |
| `404`       | Resource not found            |
| `500`       | Internal server error         |

> **Note:** Exact response codes and response bodies should be verified against the implementation.

---

## 12. API Security

Cognia uses:

* **ASP.NET Identity**
* **JWT authentication**
* **Role-based authorization** where applicable

Protected endpoints should only be accessed by authenticated and authorized users.

Sensitive credentials such as **JWT secrets** and **Paystack secret keys** must not be committed to source control.
