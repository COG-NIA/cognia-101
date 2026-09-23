# Cognia - Project Management Board
**Deadline: October 5, 2026 (11:59 PM)**

## Team Members & Roles

| # | Student ID | Name | Role |
|---|------------|------|------|
| 1 | 22031632 | Papah Kweku Okae Quansah | Team Lead & Full-Stack Developer |
| 2 | 22032918 | Noye Magdalene Norkai | UI/UX Designer & Blazor Developer |
| 3 | 22100696 | Beatrice Bansah | Backend Developer (ASP.NET Core API) |
| 4 | 22241111 | Asare Boateng Abel | Database Administrator (EF Core / SQL Server) |
| 5 | 22049777 | Adade Priscilla Adwoa | Frontend Developer & Accessibility Lead |
| 6 | 22242639 | Marie-Anne Dzifa Hayibor | Community Forum Module Developer |
| 7 | 22044680 | Lois Osei-Bonsu | Payment & Therapist Booking Integration |
| 8 | 22124731 | Josephine Tetteh | Real-Time Features Developer (SignalR) |
| 9 | 22182128 | Tetteh Joel Oglie Nathan | Testing & Quality Assurance Lead |
| 10 | 22188216 | Akubia Edith Elorm | Documentation & Report Writer |
| 11 | 22018183 | Edmond Dare | DevOps & Deployment (Azure / Docker) |
| 12 | 22250179 | Vical Divine Aghe | Security & Authentication (ASP.NET Identity) |

---

## Week 1: Sept 22-29 - Core Setup & MVP

### Day 1-2 (Sept 22-23): Infrastructure Setup

#### Asare Boateng Abel (Database)
- [ ] Design SQL Server database schema
- [ ] Create EF Core DbContext
- [ ] Define entity models (Users, MoodEntries, Articles, ForumThreads, Therapists, Sessions, Payments)
- [ ] Create initial migration
- [ ] Set up database connection string

#### Vical Divine Aghe (Security)
- [ ] Set up ASP.NET Identity
- [ ] Configure user roles (Admin, User, Counsellor, Therapist)
- [ ] Implement JWT authentication
- [ ] Set up authorization policies
- [ ] Configure password hashing

#### Edmond Dare (DevOps)
- [ ] Initialize GitHub repository
- [ ] Set up Azure resources (App Service, SQL Server)
- [ ] Configure Docker containers
- [ ] Set up CI/CD pipeline (GitHub Actions)
- [ ] Configure environment variables

#### Papah Kweku Okae Quansah (Team Lead)
- [ ] Create solution structure (completed)
- [ ] Set up project folders
- [ ] Add NuGet packages
- [ ] Coordinate team tasks
- [ ] Create coding standards document

---

### Day 3-5 (Sept 24-26): Core Module Development

#### Beatrice Bansah (Backend)
- [ ] Create API controllers for Users
- [ ] Create API controllers for Mood Tracker
- [ ] Create API controllers for Self-Help Articles
- [ ] Implement CRUD operations
- [ ] Add API documentation (Swagger)

#### Adade Priscilla Adwoa (Frontend)
- [ ] Create Blazor page layout
- [ ] Implement navigation menu
- [ ] Create Mood Tracker UI
- [ ] Create Self-Help Hub UI
- [ ] Implement responsive design

#### Noye Magdalene Norkai (UI/UX)
- [ ] Design mood input interface
- [ ] Design dashboard layout
- [ ] Create color scheme and typography
- [ ] Design article display cards
- [ ] Create accessibility features

#### Marie-Anne Dzifa Hayibor (Forum)
- [ ] Design forum data model
- [ ] Create thread/reply API endpoints
- [ ] Implement forum UI structure
- [ ] Add topic categories
- [ ] Create thread listing page

#### Josephine Tetteh (SignalR)
- [ ] Set up SignalR hub
- [ ] Implement real-time reply notifications
- [ ] Create notification service
- [ ] Add connection management
- [ ] Test real-time features

#### Lois Osei-Bonsu (Payment/Booking)
- [ ] Create Therapist model
- [ ] Implement therapist profile API
- [ ] Create booking UI
- [ ] Set up Paystack account
- [ ] Design booking flow

---

### Day 6-7 (Sept 27-29): Integration

#### All Backend Developers
- [ ] Integrate all API endpoints
- [ ] Test API connectivity
- [ ] Fix CORS issues
- [ ] Implement error handling
- [ ] Add logging

#### All Frontend Developers
- [ ] Connect Blazor UI to APIs
- [ ] Implement state management
- [ ] Add loading states
- [ ] Handle API errors
- [ ] Test user flows

#### Tetteh Joel Oglie Nathan (QA)
- [ ] Create test plan
- [ ] Start unit testing
- [ ] Begin integration testing
- [ ] Track initial bugs
- [ ] Set up bug tracking

---

## Week 2: Sept 30 - Oct 5 - Polish & Submission

### Day 8-9 (Sept 30-Oct 1): Advanced Features

#### Lois Osei-Bonsu (Payment)
- [ ] Integrate Paystack API
- [ ] Implement payment processing
- [ ] Add payment confirmation
- [ ] Create payment history
- [ ] Test payment flow

#### Josephine Tetteh (SignalR)
- [ ] Implement check-in notifications
- [ ] Add daily reminder system
- [ ] Create anonymous support routing
- [ ] Test notification delivery
- [ ] Optimize SignalR performance

#### Marie-Anne Dzifa Hayibor (Forum)
- [ ] Implement anonymous posting
- [ ] Add content moderation
- [ ] Create admin moderation panel
- [ ] Add trigger warnings
- [ ] Implement emoji reactions

#### Vical Divine Aghe (Security)
- [ ] Implement content moderation security
- [ ] Add rate limiting
- [ ] Secure anonymous features
- [ ] Implement CSRF protection
- [ ] Security audit

#### Beatrice Bansah (Backend)
- [ ] Implement anonymous support API
- [ ] Add help request routing
- [ ] Create counsellor dashboard
- [ ] Optimize API performance
- [ ] Add caching

---

### Day 10-11 (Oct 2-3): Testing & Bug Fixes

#### Tetteh Joel Oglie Nathan (QA)
- [ ] Full system testing
- [ ] Cross-browser testing
- [ ] Mobile responsiveness testing
- [ ] Performance testing
- [ ] Security testing

#### All Members
- [ ] Fix assigned bugs
- [ ] Address QA findings
- [ ] Refactor code
- [ ] Optimize database queries
- [ ] Clean up code

#### Akubia Edith Elorm (Documentation)
- [ ] Write user documentation
- [ ] Document API endpoints
- [ ] Create setup guide
- [ ] Write developer guide
- [ ] Start final report

---

### Day 12-13 (Oct 4-5): Final Prep & Submission

#### Edmond Dare (DevOps)
- [ ] Deploy to production
- [ ] Configure production database
- [ ] Set up monitoring
- [ ] Test deployment
- [ ] Verify all links work

#### Akubia Edith Elorm (Documentation)
- [ ] Complete final report
- [ ] Create presentation slides
- [ ] Document architecture
- [ ] Finalize all documentation
- [ ] Review submission requirements

#### Papah Kweku Okae Quansah (Team Lead)
- [ ] Final code review
- [ ] Verify GitHub repo is accessible
- [ ] Coordinate video recording
- [ ] Final submission checklist
- [ ] Submit Google Form

#### All Members
- [ ] Record group video (all members visible)
- [ ] Explain individual contributions
- [ ] Upload video to YouTube (Unlisted)
- [ ] Verify video plays in incognito
- [ ] Final sign-off

---

## MVP Priority (Must Complete Before Deadline)

1. ✅ User authentication (Login/Register)
2. ✅ Mood Tracker (Log mood + view basic chart)
3. ✅ Self-Help Hub (Display articles)
4. ✅ Safe Space Forum (Create threads + reply)
5. ✅ Therapist Profiles (View profiles + basic booking)

## Nice-to-Have (If Time Permits)

- Paystack payment integration
- Real-time notifications
- Anonymous support routing
- Advanced charts/analytics
- Video session integration (Jitsi)
- SMS notifications

---

## Submission Checklist

- [ ] GitHub repository is public and accessible
- [ ] Backend deployed and working
- [ ] Frontend deployed and working
- [ ] Database is accessible
- [ ] All links work in incognito mode
- [ ] Group video recorded (all members visible)
- [ ] Video uploaded to YouTube as Unlisted
- [ ] Video link works in incognito mode
- [ ] Google Form submitted by group leader only
- [ ] All team member names and IDs included

---

## Communication Channels

- **GitHub Issues**: Bug tracking and feature requests
- **Group Chat**: Daily standups and quick questions
- **Weekly Meetings**: Every Sunday 7 PM GMT

## Important Notes

- Each member should update their tasks daily
- Report blockers immediately to Team Lead
- Code reviews required before merging
- All commits must have descriptive messages
- Test thoroughly before marking tasks complete
