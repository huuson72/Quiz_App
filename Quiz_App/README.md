# Quiz Application API

A .NET Core Web API for managing quizzes with questions, answers, and tracking user attempts.

## Entity-Relationship Diagram

```
+------------+       +------------+       +------------+
|    Quiz    |       |  Question  |       |   Answer   |
+------------+       +------------+       +------------+
| Id         |<----->| Id         |<----->| Id         |
| Title      |       | Text       |       | Text       |
| Description|       | QuizId     |       | IsCorrect  |
| PassingScore|      |            |       | QuestionId |
+------------+       +------------+       +------------+
      ^                    ^                    ^
      |                    |                    |
      v                    v                    v
+------------+       +---------------+
|QuizAttempt |       | UserResponse  |
+------------+       +---------------+
| Id         |<----->| Id            |
| UserId     |       | QuizAttemptId |
| QuizId     |       | QuestionId    |
| StartTime  |       | AnswerId      |
| EndTime    |       | IsCorrect     |
| Score      |       | ResponseTime  |
+------------+       +---------------+
```

## Database Schema

The application uses the following entity models:

1. **Quiz**: Represents a quiz with multiple questions.
2. **Question**: Represents a question belonging to a quiz.
3. **Answer**: Represents a possible answer for a question.
4. **QuizAttempt**: Tracks a user's attempt at completing a quiz.
5. **UserResponse**: Records a user's answer to a specific question during a quiz attempt.

## API Endpoints

### Quiz Management

- `GET /api/Quiz`: Returns a list of all available quizzes.
- `GET /api/Quiz/{id}`: Returns details of a specific quiz including its questions and answers.
- `POST /api/Quiz/Start`: Starts a new quiz attempt for a user.

### Question Management

- `GET /api/Question/{id}`: Returns a specific question with its possible answers.
- `POST /api/Question/SubmitAnswer`: Submits an answer for a question in an active quiz attempt.

### Results

- `GET /api/Result/{quizAttemptId}`: Returns the detailed results of a completed quiz attempt.

## Quiz Flow

1. The user starts a quiz by calling the `POST /api/Quiz/Start` endpoint.
2. The API returns the first question ID of the quiz.
3. The user submits an answer to a question using the `POST /api/Question/SubmitAnswer` endpoint.
4. The API validates the answer and returns feedback to the user.
5. If more questions remain, the API provides the next question ID.
6. Once all questions have been answered, the quiz is marked as complete.
7. The user can retrieve their quiz results using the `GET /api/Result/{quizAttemptId}` endpoint.

## Setup and Configuration

1. Configure the database connection string in `appsettings.json`.
2. Run the application to automatically create the database and seed initial data.
3. Access the Swagger UI at `/swagger` to test the API endpoints. 