import { useContext } from "react";
import { Button, Card, Container } from "react-bootstrap";
import { Link } from "react-router-dom";
import { BookingContext } from "./AppContext";
import { getTimeString } from "./services";

export default function Table({ table }) {
  const { bookingTableState, bookingTimePeriodState } =
    useContext(BookingContext);

  const [bookingTable, setBookingTable] = bookingTableState;
  const [bookingTimePeriod, setBookingTimePeriod] = bookingTimePeriodState;
  return (
    <Card>
      <Card.Body>
        <Card.Title>Table id: {table.id}</Card.Title>
        <div>{table.seatNumber} seats</div>
        <p>{table.description}</p>
        {table.diningPeriods.length === 0 ? (
          <h5 className="text-danger">Booked out</h5>
        ) : (
          table.diningPeriods.map((freePeriod) => (
            <Button
              className="m-1"
              variant={`${
                bookingTable.id == table.id &&
                bookingTimePeriod.timeStartMinutes ==
                  freePeriod.timeStartMinutes
                  ? "warning"
                  : "primary"
              }`}
              onClick={() => {
                setBookingTable(table);
                setBookingTimePeriod(freePeriod);
              }}
            >
              {getTimeString(freePeriod.timeStartMinutes)}
            </Button>
          ))
        )}
        <div className="d-flex justify-content-end">
          <Link to="/booking-confirmation">
            <Button variant="primary">Book</Button>
          </Link>
        </div>
      </Card.Body>
    </Card>
  );
}
