# Meter Reading API

A RESTful API for managing meter readings, including uploading and processing CSV files, and seeding accounts in the database.

## Endpoints

### `=== MeterReadingController ===`
### `POST /meter-reading-uploads`
- **Description**: Upload and process a CSV file with meter readings.
- **Request Body**: `multipart/form-data` with a file (`.csv`).
- **Response**: 
  - `200 OK`: File processed successfully.
  - `400 Bad Request`: Invalid file or file content.
  - `500 Internal Server Error`: Error during file processing.


### `POST /seed-accounts`
- **Description**: Upload and seed account data from a CSV file.
- **Request Body**: `multipart/form-data` with a file (`.csv`).
- **Response**:
  - `200 OK`: Accounts successfully uploaded and saved.
  - `400 Bad Request`: Invalid file format or empty file.
  - `500 Internal Server Error`: Error during seeding.




### `=== EnsekController ===`
### `GET /all-meter-readings`
- **Description**: Get all meter readings with optional pagination.
- **Query Parameters**:
  - `page`: The page number (default is `1`).
  - `pageSize`: The number of records per page (default is `10`).
- **Response**:
  - `200 OK`: Paginated list of meter readings.
  - `404 Not Found`: No meter readings found.
	- 