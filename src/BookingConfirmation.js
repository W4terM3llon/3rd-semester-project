import { useContext, useState } from "react";
import {
  Card,
  ListGroup,
  Button,
  Col,
  Container,
  Alert,
  Row,
} from "react-bootstrap";
import { BookingContext, JwtTokenContext } from "./AppContext";
import configData from "./config.json";

export default function BookingConfirmation() {
  const { bookingTableState, bookingTimePeriodState, bookingDateState } =
    useContext(BookingContext);
  const { jwtTokenState } = useContext(JwtTokenContext);

  const [jwtToken, setJwtToken] = jwtTokenState;
  const [bookingDate, setBookingDate] = bookingDateState;
  const [bookingTable, setBookingTable] = bookingTableState;
  const [bookingTimePeriod, setBookingTimePeriod] = bookingTimePeriodState;

  const [showBookingError, setShowBookingError] = useState(false);
  const [showBookingSuccess, setShowBookingSuccess] = useState(false);

  function confirmBooking() {
    fetch(`${configData.SERVER_URL}api/Booking/`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
      body: JSON.stringify({
        date: bookingDate,
        table: bookingTable.id,
        diningPeriod: bookingTimePeriod.id,
        user: "104",
        restaurant: bookingTable.restaurant.id,
      }),
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
            .catch((err) => {setShowBookingError(true)})
        );
      })
      .then((data) => {
        if (data.ok) {
          setShowBookingError(false);
          setShowBookingSuccess(true);
        } else {
          setShowBookingError(true);
          setShowBookingSuccess(false);
        }
      })
      .catch((err) => {});
  }

  return (
    <Container className="d-flex justify-content-center align-items-center mt-5">
      <Col xs={12} md={8} xl={4}>
        <Row>
          <Card className="p-3">
            <Card.Title className="text-center">Confirm your booking</Card.Title>
            <ListGroup>
              <ListGroup.Item>Date: {bookingDate}</ListGroup.Item>
              <ListGroup.Item>
                Time: {bookingTimePeriod.timeStartMinutes}
              </ListGroup.Item>
              <ListGroup.Item>
                Restaurant: {bookingTable.restaurant.name}
              </ListGroup.Item>
              <ListGroup.Item>
                Table seats: {bookingTable.seatNumber}
              </ListGroup.Item>
            </ListGroup>
            <Button onClick={confirmBooking} className="mt-2">Confirm</Button>
          </Card>
        </Row>
        <Row>
          <Alert
            variant="warning"
            className="mt-2"
            show={showBookingError}
            onClose={() => setShowBookingError(false)}
            dismissible
          >
            <Alert.Heading>Could not place a booking</Alert.Heading>
          </Alert>
          <Alert
            variant="success"
            className="mt-2"
            show={showBookingSuccess}
            onClose={() => setShowBookingSuccess(false)}
            dismissible
          >
            <Alert.Heading>Successfull booking</Alert.Heading>
          </Alert>
        </Row>
      </Col>
    </Container>
  );
}
