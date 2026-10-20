# Cognia API Documentation
**Project:** Cognia (Student Mental Health Platform)  
**Updated:** October 20, 2026

---

## 1. Overview

Cognia provides a REST API using **ASP.NET Core Web API on .NET 10**. MongoDB stores accounts, articles, forum threads, mood entries, therapist profiles, sessions, payments, and ID sequences.

The API supports:

* JWT account registration and login
* Authenticated, per-account mood tracking
* Public self-help articles
* Public forum reads and authenticated posts/replies
* Public therapist directory and authenticated bookings
* Authenticated payment initialization/history/verification

---

## 2. Base URL

During local development, the backend API runs at:

```text
https://localhost:7214
```

Therefore, an endpoint such as:

```http
POST /api/auth/login
```

is accessed locally through the API server.

---

## 3. Authentication

Cognia hashes passwords with ASP.NET Core's `PasswordHasher<T>` and issues JWT bearer tokens. Send protected requests with `Authorization: Bearer <token>`. Configure `MongoDb:ConnectionString` and a `Jwt:SigningKey` of at least 32 bytes using user-secrets locally or environment variables in hosting; do not commit secrets.

### 3.1 Bootstrap the First Administrator

**Endpoint:** `POST /api/auth/bootstrap-admin`

This one-time endpoint requires the `X-Admin-Bootstrap-Token` header to match `Admin:BootstrapToken`. The JSON body is `{ "email": "...", "displayName": "...", "password": "..." }`; passwords must be at least 12 characters. It returns an auth response and refuses to create another administrator once one exists. Use the administrator to add actual therapist and article records.

### 3.2 Register User

**Endpoint**

```http
POST /api/auth/register
```

**Purpose:**
Creates a new Cognia user account.

**Method:** `POST`

**Authentication:** Not required for registration.

---

### 3.3 Login

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

The browser client stores the token for the current tab session. Signing out removes that token client-side; there is no server logout endpoint.
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

Mood scores are integers from 1 to 5. Requests store the authenticated account ID; a caller cannot read or write another user's entries.

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

### 5.3 Create Article

**Endpoint:** `POST /api/articles`
**Authentication:** Admin role required.

The database contains no seeded articles. Add only approved, genuine content through this endpoint.

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

**Authentication:** Required. Author display name is derived from the account; `IsAnonymous` hides it in the forum UI.

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

**Authentication:** Required. The author is derived from the account.

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

Forum categories are predefined in the API. New threads and replies are stored in MongoDB.

---

## 7. Therapist API

### 7.1 Get Therapists

**Endpoint**

```http
GET /api/therapists
```

**Purpose:**
Retrieves therapist profiles stored in MongoDB. The collection starts empty; there are no sample therapists.

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

### 7.3 Add Therapist

**Endpoint:** `POST /api/therapists`
**Authentication:** Admin role required.

Only enter real, authorized therapist details and mark `IsVerified` true after verification.

---

## 8. Session API

All session routes require authentication and return only the signed-in user's sessions.

### 8.1 Get My Sessions

```http
GET /api/sessions
```

### 8.2 Get My Session

```http
GET /api/sessions/{id}
```

### 8.3 Book Session

**Endpoint**

```http
POST /api/sessions
```

**Purpose:**
Creates a therapist-session booking.

**Method:** `POST`

The server requires a therapist record from MongoDB and calculates the amount from that record. Session details are stored in MongoDB.

---

## 9. Payment API

### 9.1 Get My Payment History

`GET /api/payments` (authenticated; only the caller's records)

### 9.2 Initialize Payment

`POST /api/payments/initialize` (authenticated)

The API uses its configured `Paystack:SecretKey`; amount and customer email are read from the owned session, not trusted from the client. Successful initialization creates a pending MongoDB payment record and returns Paystack's authorization URL.

### 9.3 Verify Payment

`GET /api/payments/verify/{reference}` (authenticated; reference must belong to the caller)

The payment is marked successful and its session confirmed only when Paystack returns a successful status with the matching reference and amount. Missing configuration/provider errors do not produce a simulated payment.

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

## 11. API Security

Cognia uses:

* MongoDB-backed account records and password hashes
* **JWT authentication**
* **Role-based authorization** for therapist/article authoring
* Per-account filtering for mood, session, and payment records
* A one-time configuration-token-protected admin bootstrap

Protected endpoints should only be accessed by authenticated and authorized users.

MongoDB connection strings, JWT signing keys, admin bootstrap tokens, and Paystack secrets must be configured through user-secrets locally and App Service settings in Azure. The database is intentionally empty on first startup.
