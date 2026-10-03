# Cognia - Master Test Plan
**Project Name:** Cognia Mental Health & Student Support Platform  
**Author:** Tetteh Joel Oglie Nathan (Testing & Quality Assurance Lead)  
**Student ID:** 22182128  
**Date:** October 3, 2026  
**Version:** 1.0  

---

## 1. Executive Summary

As the **Testing & Quality Assurance Lead**, this Master Test Plan outlines the testing strategy, test methodology, execution scope, and quality criteria for the **Cognia** web application. 

The primary objective is to verify that all functional requirements, security standards, API endpoint behaviors, and user interface responsiveness perform reliably prior to production deployment.

---

## 2. Testing Objectives & Scope

### 2.1 In-Scope Requirements
- **Backend API Unit & Integration Testing**: `Cognia.API` controllers (e.g. `ForumController`), route bindings, model validations, error handling (400 Bad Request, 404 Not Found), and HTTP responses.
- **Data Model & Logic Verification**: `Cognia.Shared` domain models (`ForumThread`, `ForumReply`, `ForumCategories`).
- **Security & Input Handling**: Input trimming, null check safety, anonymous posting routing, and prevention of basic injection patterns.
- **Cross-Browser & Responsiveness Validation**: Verification across desktop, tablet, and mobile breakpoints (Chrome, Firefox, Edge, Safari).
- **Performance & Concurrency**: Response timing and simultaneous request handling.

---

## 3. Test Strategy & Methodology

### 3.1 Automated Testing Framework
- **Test Framework**: xUnit (`.NET 10.0`)
- **Test Project**: `Cognia.Tests`
- **Assertion Library**: xUnit Assertions

### 3.2 Test Categories & Suites

| Test Suite | File | Focus Area | Total Tests | Status |
|------------|------|------------|-------------|--------|
| **Controller Tests** | `ForumControllerTests.cs` | API endpoints, HTTP status codes, payload handling | 13 | ✅ PASS |
| **Domain Model Tests** | `ForumModelTests.cs` | Model initialization, defaults, category collections | 7 | ✅ PASS |
| **Security & Input Tests** | `SecurityAndSanitizationTests.cs` | Input trimming, boundary testing, special characters | 4 | ✅ PASS |

---

## 4. Test Execution Summary

```text
Test Run Summary: Cognia.Tests.dll (.NETCoreApp, Version=v10.0)
Total Tests Run: 24
Passed: 24
Failed: 0
Skipped: 0
Duration: 2.0 seconds
Pass Rate: 100.0%
```

---

## 5. Defect Management & Quality Gate Criteria

### 5.1 Severity Definitions
- **Critical (P0)**: Application crash, security vulnerability, data loss.
- **Major (P1)**: Core feature broken without workaround.
- **Minor (P2)**: Non-critical feature bug, minor UI misalignment.
- **Low (P3)**: Cosmetic defects, typo fixes.

### 5.2 Quality Gate Standard
For release sign-off:
- Zero open P0 / P1 defects.
- 100% unit test execution pass rate on `Cognia.Tests`.
- All API endpoints returning standard HTTP status codes (`200 OK`, `201 Created`, `400 Bad Request`, `404 Not Found`).

---

## 6. Sign-off

**Lead QA Auditor:** Tetteh Joel Oglie Nathan  
**Role:** Quality Assurance Lead  
**Status:** Approved for System Integration & Deployment  
