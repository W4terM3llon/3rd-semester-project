import React, { useEffect, useReducer, useState } from "react";
import { getJWT, setJWT } from "./services";
import configData from "./config.json";

export const JwtTokenContext = React.createContext();
export const UserContext = React.createContext();
export const RestaurantsContext = React.createContext();
export const DishCategoriesContext = React.createContext();
export const ChosenRestaurantContext = React.createContext();
export const BookingContext = React.createContext();
export const OrderContext = React.createContext();

export default function AppContext({ children }) {
  const [jwtToken, setJwtToken] = useState(getJWT());

  const [userId, setUserId] = useState("");
  const [userFirstName, setUserFirstName] = useState("");
  const [userLastName, setUserLastName] = useState("");
  const [userEmail, setUserEmail] = useState("");
  const [userPhoneNumber, setUserPhoneNumber] = useState("");
  const [userAddressStreet, setUserAddressStreet] = useState("");
  const [userAddressAppartment, setUserAddressAppartment] = useState("");

  const [availableRestaurants, setAvailableRestaurants] = useState([]);
  const [dishCategories, setDishCategories] = useState([]);

  const [chosenRestaurantId, setChosenRestaurantId] = useState(-1);
  const [chosenRestaurantDishes, setChosenRestaurantDishes] = useState([]);
  const [chosenRestaurantTables, setChosenRestaurantTables] = useState([]);

  const [bookingDate, setBookingDate] = useState(
    new Date().toISOString().split("T")[0]
  );
  const [bookingTable, setBookingTable] = useState({});
  const [bookingTimePeriod, setBookingTimePeriod] = useState({});

  const [orderLines, setOrderLines] = useState([])

  const [bookingHistory, setBookingHistory] = useState([]);
  const [orderHistory, setOrderHistory] = useState([]);

  /*console.log(orderLinesState);
  function orderLineReducer(state, action) {
    switch (action.type) {
      case "add":
        return {
          orderLines: [
            ...state.orderLines,
            { dish: action.dish, quantity: action.quantity },
          ],
        };
      case "increase":
        let orderlinesIncrease = [...state.orderLines];
        orderlinesIncrease.filter(
          (orderLine) => orderLine.dish.id == action.dish.id
        )[0].quantity += 1;
        console.log(
          orderlinesIncrease.filter(
            (orderLine) => orderLine.dish.id == action.dish.id
          )[0].quantity,
          action.quantity
        );
        return { orderLines: orderlinesIncrease };
      case "remove":
        return {
          orderLines: state.orderLines.filter(
            (orderLine) => orderLine.dishId != action.orderLine.dishId
          ),
        };
      case "decrease":
        let orderlinesDecrease = [...state.orderLines];
        orderlinesDecrease.filter(
          (orderLine) => orderLine.dish.id == action.dish.id
        ).quantity -= action.quantity;
        return { orderLines: orderlinesDecrease };
      default:
        throw new Error();
    }
  }*/

  useEffect(() => {
    if (jwtToken) {
      setJWT(jwtToken);
    } else {
      setJWT("");
    }

    if (jwtToken != "") {
      fetch(`${configData.SERVER_URL}api/Users`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${jwtToken}`,
        },
      })
        .then((response) => {
          return new Promise((resolve) =>
            response
              .json()
              .then((data) =>
                resolve({
                  ok: response.ok,
                  status: response.status,
                  body: data,
                })
              )
              .catch((err) => {})
          );
        })
        .then((data) => {
          if (data.ok) {
            setUserId(data.body.systemId);
            setUserFirstName(data.body.firstName);
            setUserLastName(data.body.lastName);
            setUserEmail(data.body.email);
            setUserPhoneNumber(data.body.phoneNumber);
            setUserAddressStreet(data.body.address.street);
            setUserAddressAppartment(data.body.address.appartment);
          } else {
            setUserId("");
            setUserFirstName("");
            setUserLastName("");
            setUserEmail("");
            setUserPhoneNumber("");
            setUserAddressStreet("");
            setUserAddressAppartment("");
          }
        })
        .catch((err) => {});
    }
  }, [jwtToken]);

  function fetchOrderHistory(){
    fetch(`${configData.SERVER_URL}api/Orders?userId=${userId}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${jwtToken}`,
      },
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({
                ok: response.ok,
                status: response.status,
                body: data,
              })
            )
            .catch((err) => {})
        );
      })
      .then((data) => {
        if (data.ok) {
          setOrderHistory(data.body);
        } else {
          setOrderHistory([]);
        }
      })
      .catch((err) => {});
  }

  useEffect(() => {
    if (userId != "") {
      fetch(`${configData.SERVER_URL}api/Booking?userId=${userId}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${jwtToken}`,
        },
      })
        .then((response) => {
          return new Promise((resolve) =>
            response
              .json()
              .then((data) =>
                resolve({
                  ok: response.ok,
                  status: response.status,
                  body: data,
                })
              )
              .catch((err) => {})
          );
        })
        .then((data) => {
          if (data.ok) {
            setBookingHistory(data.body);
          } else {
            setBookingHistory([]);
          }
        })
        .catch((err) => {});

        fetchOrderHistory()
    }
  }, [userId]);

  useEffect(() => {
    if (userId != "") {
      fetch(`${configData.SERVER_URL}api/Users/${userId}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${jwtToken}`,
        },
      })
        .then((response) => {
          return new Promise((resolve) =>
            response
              .json()
              .then((data) =>
                resolve({
                  ok: response.ok,
                  status: response.status,
                  body: data,
                })
              )
              .catch((err) => {})
          );
        })
        .then((data) => {
          if (data.ok) {
            setUserFirstName(data.body.firstName);
            setUserLastName(data.body.lastName);
            setUserEmail(data.body.email);
            setUserPhoneNumber(data.body.phoneNumber);
            setUserAddressStreet(data.body.address.street);
            setUserAddressAppartment(data.body.address.appartment);
          } else {
            setUserFirstName(data.body.firstName);
            setUserLastName(data.body.lastName);
            setUserEmail(data.body.email);
            setUserPhoneNumber(data.body.phoneNumber);
            setUserAddressStreet(data.body.address.street);
            setUserAddressAppartment(data.body.address.appartment);
          }
        })
        .catch((err) => {});
    }
  }, [userId]);

  function fetchTablesFreePeriods(currentTables) {
    fetch(
      `${configData.SERVER_URL}api/TableFreePeriods?RestaurantId=${chosenRestaurantId}&date=${bookingDate}`,
      {
        method: "GET",
      }
    )
      .then((response) => {
        return new Promise((resolve) =>
          response.json().then((data) =>
            resolve({
              ok: response.ok,
              status: response.status,
              body: data,
            })
          )
        );
      })
      .then((data) => {
        if (data.ok) {
          var tables = [...currentTables];
          tables.forEach((table) => {
            table["diningPeriods"] = data.body[table.id.toString()];
          });
          console.log(tables);
          setChosenRestaurantTables(tables);
        } else {
          console.log("could not fetch tables free periods");
        }
      })
      .catch((err) => {});
  }

  useEffect(()=>{
    fetchTablesFreePeriods(chosenRestaurantTables)
  }, [bookingDate])

  useEffect(() => {
    fetch(
      `${configData.SERVER_URL}api/Dishes?RestaurantId=${chosenRestaurantId}`,
      {
        method: "GET",
      }
    )
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
        );
      })
      .then((data) => {
        if (data.ok) {
          setChosenRestaurantDishes(data.body);
        } else {
          console.log("could not fetch restaurant dishes");
        }
      })
      .catch((err) => {});

    fetch(
      `${configData.SERVER_URL}api/Tables?RestaurantId=${chosenRestaurantId}`,
      {
        method: "GET",
      }
    )
      .then((response) => {
        return new Promise((resolve) =>
          response.json().then((data) =>
            resolve({
              ok: response.ok,
              status: response.status,
              body: data,
            })
          )
        );
      })
      .then((data) => {
        if (data.ok) {
          fetchTablesFreePeriods(data.body);
        } else {
          console.log("could not fetch restaurant tables");
        }
      })
      .catch((err) => {});
  }, [chosenRestaurantId]);

  useEffect(() => {
    //fetch all restaurants
    fetch(`${configData.SERVER_URL}api/Restaurant/`, {
      method: "GET",
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
        );
      })
      .then((data) => {
        if (data.ok) {
          setAvailableRestaurants(data.body);
        } else {
          console.log("could not fetchrestaurants");
        }
      })
      .catch((err) => {});

    //fetch all dish categories
    fetch(`${configData.SERVER_URL}api/DishCategories`, {
      method: "GET",
    })
      .then((response) => {
        return new Promise((resolve) =>
          response
            .json()
            .then((data) =>
              resolve({ ok: response.ok, status: response.status, body: data })
            )
        );
      })
      .then((data) => {
        if (data.ok) {
          console.log(data.body);
          setDishCategories(data.body);
        } else {
          console.log("could not fetch dish categories");
        }
      })
      .catch((err) => {});
  }, []);

  function logOutUser() {
    setJwtToken("");
    setUserId("");
    setUserFirstName("");
    setUserLastName("");
    setUserEmail("");
    setUserPhoneNumber("");
    setUserAddressStreet("");
    setUserAddressAppartment("");
  }

  return (
    <JwtTokenContext.Provider
      value={{ jwtTokenState: [jwtToken, setJwtToken] }}
    >
      <UserContext.Provider
        value={{
          userIdState: [userId, setUserId],
          userFirstNameState: [userFirstName, setUserFirstName],
          userLastNameState: [userLastName, setUserLastName],
          userEmailState: [userEmail, setUserEmail],
          userPhoneNumberState: [userPhoneNumber, setUserPhoneNumber],
          userAddressStreetState: [userAddressStreet, setUserAddressStreet],
          userAddressAppartmentState: [
            userAddressAppartment,
            setUserAddressAppartment,
          ],
          logOutUser: logOutUser,
        }}
      >
        <RestaurantsContext.Provider
          value={{
            availableRestaurantsState: [
              availableRestaurants,
              setAvailableRestaurants,
            ],
          }}
        >
          <DishCategoriesContext.Provider
            value={{
              dishCategoriesState: [dishCategories, setDishCategories],
            }}
          >
            <ChosenRestaurantContext.Provider
              value={{
                chosenRestaurantIdState: [
                  chosenRestaurantId,
                  setChosenRestaurantId,
                ],
                chosenRestaurantDishesState: [
                  chosenRestaurantDishes,
                  setChosenRestaurantDishes,
                ],
                chosenRestaurantTablesState: [
                  chosenRestaurantTables,
                  setChosenRestaurantTables,
                ],
                fetchTablesFreePeriods: fetchTablesFreePeriods,
              }}
            >
              <OrderContext.Provider
                value={{
                  orderLinesState: [orderLines, setOrderLines],
                  orderHistoryState: [orderHistory, setOrderHistory],
                  fetchOrderHistory: fetchOrderHistory
                }}
              >
                <BookingContext.Provider
                  value={{
                    bookingDateState: [bookingDate, setBookingDate],
                    bookingTableState: [bookingTable, setBookingTable],
                    bookingTimePeriodState: [
                      bookingTimePeriod,
                      setBookingTimePeriod,
                    ],
                    bookingHistoryState: [bookingHistory, setBookingHistory],
                  }}
                >
                  {children}
                </BookingContext.Provider>
              </OrderContext.Provider>
            </ChosenRestaurantContext.Provider>
          </DishCategoriesContext.Provider>
        </RestaurantsContext.Provider>
      </UserContext.Provider>
    </JwtTokenContext.Provider>
  );
}
