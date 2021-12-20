import { useContext, useEffect, useState } from "react";
import { Card, Col, Container, Form, Row, Tab } from "react-bootstrap";
import { Link } from "react-router-dom";
import {
  BookingContext,
  ChosenRestaurantContext,
  RestaurantsContext,
} from "./AppContext";
import Dish from "./Dish";
import Table from "./Table";

export default function Restaurants() {
  const {
    chosenRestaurantDishesState,
    chosenRestaurantTablesState,
    chosenRestaurantIdState,
  } = useContext(ChosenRestaurantContext);
  const { availableRestaurantsState } = useContext(RestaurantsContext);
  const { bookingDateState } = useContext(BookingContext);

  const [availableRestaurants, setAvailableRestaurants] =
    availableRestaurantsState;
  const [chosenRestaurantId, setChosenRestaurantId] = chosenRestaurantIdState;
  const [chosenRestaurantDishes, setChosenRestaurantDishes] =
    chosenRestaurantDishesState;
  const [chosenRestaurantTables, setChosenRestaurantTables] =
    chosenRestaurantTablesState;

  const [bookingDate, setBookingDate] = bookingDateState;

  const [restaurantDetails, setRestaurantDetails] = useState({});
  const [tablesBySeatNumber, setTablesBySeatNumber] = useState([]);
  const [dishesByCategory, setDishesByCategory] = useState([]);

  const [showTables, setShowTables] = useState(true);
  const [showDishes, setShowDishes] = useState(false);
  const [tablesToShow, setTablesToShow] = useState(chosenRestaurantTables);
  const [dishesToShow, setDishesToShow] = useState(chosenRestaurantDishes);

 
  useEffect(() => {
    var restaurant = availableRestaurants.filter(
      (restaurant) => restaurant.id == chosenRestaurantId
    );
    if (restaurant !== undefined) {
      setRestaurantDetails(restaurant[0]);
    } else {
      setRestaurantDetails({});
    }
  }, [chosenRestaurantId, availableRestaurants]);

  useEffect(() => {
    var dishesByCategoryTemp = {};
    chosenRestaurantDishes.map((dish) => {
      if (dish.dishCategory.name in dishesByCategoryTemp) {
        dishesByCategoryTemp[dish.dishCategory.name].push(dish);
      } else {
        dishesByCategoryTemp[dish.dishCategory.name] = [dish];
      }
    });
    var convertedTables = Object.entries(dishesByCategoryTemp);
    setDishesByCategory(convertedTables);

    setDishesToShow(chosenRestaurantDishes);
  }, [chosenRestaurantDishes]);

  useEffect(() => {
    var tablesBySeatNumberTemp = {};
    chosenRestaurantTables.map((table) => {
      if (table.seatNumber.toString() in tablesBySeatNumberTemp) {
        tablesBySeatNumberTemp[table.seatNumber.toString()].push(table);
      } else {
        tablesBySeatNumberTemp[table.seatNumber.toString()] = [table];
      }
    });
    var convertedTables = Object.entries(tablesBySeatNumberTemp);
    setTablesBySeatNumber(convertedTables);

    setTablesToShow(chosenRestaurantTables);
  }, [chosenRestaurantTables]);

  useEffect(() => {}, [chosenRestaurantTables]);

  function dishFilterClicked(dish) {
    setDishesToShow(
      chosenRestaurantDishes.filter((dh) => dh.dishCategory.name == dish[0])
    );
    setShowTables(false);
    setShowDishes(true);
  }

  function tableFilterClicked(table) {
    setTablesToShow(
      chosenRestaurantTables.filter((tb) => tb.seatNumber == table[0])
    );
    setShowTables(true);
    setShowDishes(false);
  }

  return (
    <Container className="mt-5">
      {restaurantDetails == undefined ? (
        <Row>
          <Col className="text-center"><h5>Restaurant not found</h5></Col>
        </Row>
      ) : (
        <>
          <h2 className="text-center p-0">{restaurantDetails.name}</h2>
          <Row>
            <Col xs={3}>
              <Container className="rounded bg-light">
                <Col>
                  {restaurantDetails.isTableBookingEnabled ? (
                    <>
                      <h4 className="text-center">Tables</h4>
                      <h5 className="text-muted fs-5">By seat number</h5>
                      <Form.Group>
                        <Form.Control
                          type="date"
                          value={bookingDate}
                          onChange={(e) => {
                            setBookingDate(e.target.value);
                            setShowTables(true);
                            setShowDishes(false);
                          }}
                        />
                      </Form.Group>
                      {tablesBySeatNumber.map((table, index) => (
                        <Row className="my-1">
                          <Col>
                            <span
                              onClick={() => {
                                tableFilterClicked(table);
                              }}
                              style={{ cursor: "pointer" }}
                            >
                              {table[0]} seats ({table[1].length})
                            </span>
                          </Col>
                        </Row>
                      ))}
                      <Row>
                        <Col>
                          <span
                            onClick={() => {
                              setTablesToShow(chosenRestaurantTables);
                              setShowTables(true);
                              setShowDishes(false);
                            }}
                            style={{ cursor: "pointer" }}
                          >
                            show all
                          </span>
                        </Col>
                      </Row>
                    </>
                  ) : (
                    ""
                  )}
                  <h4 className="text-center">Dishes</h4>
                  <h5 className="text-muted fs-5">By category</h5>
                  {dishesByCategory.map((dish, index) => (
                    <Row className="my-1">
                      <Col>
                        <span
                          onClick={() => {
                            dishFilterClicked(dish);
                          }}
                          style={{ cursor: "pointer" }}
                        >
                          {dish[0]} ({dish[1].length})
                        </span>
                      </Col>
                    </Row>
                  ))}
                  <Row>
                    <Col>
                      <span
                        onClick={() => {
                          setDishesToShow(chosenRestaurantDishes);
                          setShowTables(false);
                          setShowDishes(true);
                        }}
                        style={{ cursor: "pointer" }}
                      >
                        show all
                      </span>
                    </Col>
                  </Row>
                </Col>
              </Container>
            </Col>
            <Col xs={9}>
              {showTables
                ? tablesToShow.map((table) => (
                    <Row className="mb-2">
                      <Table table={table} />
                    </Row>
                  ))
                : ""}
              {showDishes
                ? dishesToShow.map((dish) => (
                    <Row className="mb-2">
                      <Dish dish={dish} />
                    </Row>
                  ))
                : ""}
            </Col>
          </Row>
        </>
      )}
    </Container>
  );
}
