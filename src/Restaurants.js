import { useContext } from "react";
import { Card, Col, Container, Row } from "react-bootstrap";
import { Link } from "react-router-dom";
import { ChosenRestaurantIdContext, RestaurantsContext } from "./AppContext";

export default function Restaurants() {
  const { availableRestaurantsState } = useContext(RestaurantsContext);
  const { chosenRestaurantIdState } = useContext(ChosenRestaurantIdContext);

  const [availableRestaurants, setAvailableRestaurants] =
    availableRestaurantsState;
  const [chosenRestaurantId, setChosenRestaurantId] = chosenRestaurantIdState;

  return (
    <Container className="mt-5">
      <Col>
        {availableRestaurants.map((restaurant) => (
          <Row className="mb-3">
            <Link
              onClick={() => setChosenRestaurantId(restaurant.id)}
              to="/restaurant-details"
              style={{ textDecoration: "none" }}
              className="text-black"
            >
              <Card>
                <Card.Body>
                  <Card.Title>{restaurant.name}</Card.Title>
                  <Row className="py-2">
                    <Col className="d-flex justify-content-between">
                      <span>
                        Address: {restaurant.address.street}{" "}
                        {restaurant.address.appartment}
                      </span>
                      <span>
                        {restaurant.isTableBookingEnabled ? (
                          <span className="me-2 p-1">Table booking</span>
                        ) : (
                          ""
                        )}
                        {restaurant.isTableBookingEnabled ? (
                          <span className="me-2 p-1">Delivery</span>
                        ) : (
                          ""
                        )}
                      </span>
                    </Col>
                  </Row>
                </Card.Body>
              </Card>
            </Link>
          </Row>
        ))}
      </Col>
    </Container>
  );
}
