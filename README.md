
# Real-Time Scoring and Leaderboard System

This project is a scalable **real-time scoring and leaderboard system** built using **ASP.NET Core** with **.NET 8**, **Entity Framework Core**, **PostgreSQL**, and **Redis**. It allows players to submit match results, update scores, and retrieve leaderboard rankings efficiently, even under heavy traffic. The system is designed to handle high volumes of concurrent requests, maintain data consistency, and prevent data loss.

## Key Features

- **Real-Time Scoring**: Players can submit match results, and their scores are updated and reflected on the leaderboard immediately.
- **Leaderboard Ranking**: The leaderboard ranks players based on their score, experience, and trophy count. Players with the same score are ranked by additional criteria such as experience level and time of achievement.
- **Redis Caching**: Frequently accessed data, such as the top 100 players, is cached in Redis for fast retrieval, reducing database load during high traffic.
- **PostgreSQL for Data Persistence**: Player scores and leaderboard data are persisted in PostgreSQL to ensure data integrity and prevent data loss.
- **Transaction Management**: All critical operations, such as score updates and leaderboard changes, are wrapped in transactions to ensure atomicity and consistency.
- **Error Handling & Data Consistency**: The system uses retry mechanisms and transaction rollbacks to prevent data loss in case of network issues or database failures.
- **Scalability**: The system can scale horizontally to handle large volumes of traffic, with caching and optimized database queries for performance.
- **Security**: Secure API endpoints with JWT authentication, password hashing, and input validation to prevent score manipulation or unauthorized access.

## Tech Stack

- **ASP.NET Core** (.NET 8)
- **Entity Framework Core** (for data access)
- **PostgreSQL** (as the primary data store)
- **Redis** (for caching leaderboard data)
- **Microsoft Identity** (for securing API calls)
- **Docker** (for containerization and easy deployment)

## Future Enhancements

- Implementing more sophisticated ranking algorithms based on customizable criteria.
- Adding pagination support to leaderboard retrieval.
- Real-time WebSocket updates for leaderboard changes.

## Contributing

Feel free to submit issues or pull requests to improve the project! Please follow the contributing guidelines provided in the repository.

## ForRuneTeam - This Section Will be removed


System Scalability Under Heavy Traffic:

Horizontal Scaling: The application can be scaled by running multiple instances behind a load balancer, ensuring that it can handle more requests during peak usage.

Redis for Read Caching: By caching leaderboard data (such as the top 100 players) in Redis, we reduce the load on the database for frequent read operations.

Asynchronous Processing: Ensure that leaderboard updates and score submissions are processed asynchronously, so requests do not block while waiting for the database to process large volumes of updates.

Preventing Data Loss:

Transaction Management: The SubmitMatchResult method uses transactions to ensure that score updates and leaderboard updates are committed atomically. If any part of the operation fails, the transaction is rolled back, preventing partial updates.

Retry Mechanisms: Implement retries for network or database failures to ensure critical operations, such as score submissions, are retried if they fail temporarily.

Data Consistency with Redis:

Cache Invalidation: When scores are updated, invalidate the Redis cache so that the leaderboard is refreshed with the latest data from PostgreSQL.

Handling Redis Failures: In the event of a Redis crash or restart, the leaderboard can always be rebuilt from the PostgreSQL database. Redis is used for performance optimization, not as the primary data store.

Redis Persistence: Consider configuring Redis persistence options (e.g., RDB snapshots or AOF logs) to reduce the impact of data loss in case of Redis failures.


Leaderboard Update Algorithm
Real-Time Leaderboard Update:

Algorithm: After each score submission, the affected player’s score is updated, and the ranks of other players with similar or lower scores are recalculated using the UpdateAffectedRanksAsync method.

Ranking with Same Scores: If two players have the same score, additional criteria (such as when the score was achieved or the player’s experience/trophy count) will be used to determine the ranking. For example, if two players have the same score, the player with a higher experience or trophy count will be ranked higher.
Handling Ties in Ranking:

Criteria for Breaking Ties:

Player Level/Experience: Players with higher experience levels are ranked higher if their scores are tied.

Time of Achievement: If the scores and experience levels are tied, the player who achieved the score first will be ranked higher.
SQL Query: The query is already ordering by Score, TrophyCount, and Experience in descending order to handle ties.

Security:

Secure API Calls: Use JWT or OAuth2 for authentication and authorization in API calls. Ensure that only authenticated users can submit scores or access leaderboard data.

Data Encryption: Encrypt sensitive data such as passwords and session cookies. For example, UserManager in the AuthController uses hashed passwords by default, ensuring they are not stored in plain text.

Input Validation: Validate all incoming requests, especially for sensitive endpoints like score submission, to prevent SQL injection or other attacks.

Monitoring and Logging:

**Logging: Use structured logging (e.g., Serilog) to log all actions within the system, including score submissions, leaderboard updates, and authentication actions. Logs should include timestamps, user IDs, and action details for traceability.

**Redis Monitoring: Monitor Redis using tools like Redis Insights or other monitoring services. Track cache hits/misses and performance metrics to ensure Redis is functioning optimally.

**Data Integrity Checks: Regularly verify the integrity of cached data by comparing it against the source of truth (PostgreSQL). Periodically clear and refresh caches to ensure they are up-to-date.

**fields are can/will be updated

Conclusion
The system is designed to handle real-time score submissions, update leaderboards dynamically, and scale under high traffic. By combining PostgreSQL for reliable data persistence and Redis for high-performance caching, the system ensures both data integrity and efficiency.
