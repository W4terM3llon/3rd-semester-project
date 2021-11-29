import React, { useEffect, useState } from "react";
import { getJWT, setJWT } from "./services";
import configData from "./config.json";

export const JwtTokenContext = React.createContext();
export const RestaurantsContext = React.createContext();
export const DishCategoriesContext = React.createContext();
export const ChosenRestaurantIdContext = React.createContext();
export const ChosenRestaurantDishesContext = React.createContext();
export const ChosenRestaurantTablesContext = React.createContext();
export const BookingContext = React.createContext();
export const OrderContext = React.createContext();

export default function AppContext({ children }) {
  const [jwtToken, setJwtToken] = useState(getJWT());
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
  const [order, setOrder] = useState({});

  function fetchTablesFreePeriods(currentTables) {
    fetch(
      `${configData.SERVER_URL}api/TableFreePeriods?Restaurant=${chosenRestaurantId}&date=${bookingDate}`,
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
          console.log(tables)
          setChosenRestaurantTables(tables);
        } else {
          console.log("could not fetch tables free periods");
        }
      })
      .catch((err) => {});
  }

  useEffect(() => {
    setJWT(setJWT(jwtToken));
  }, [jwtToken]);

  useEffect(() => {
    fetch(
      `${configData.SERVER_URL}api/Dishes?Restaurant=${chosenRestaurantId}`,
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

    if (
      availableRestaurants.filter(
        (restaurant) => restaurant.isTableBookingEnabled
      )
    ) {
      fetch(
        `${configData.SERVER_URL}api/Tables?Restaurant=${chosenRestaurantId}`,
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
    }
  }, [chosenRestaurantId]);

  useEffect(() => {
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
  }, []);

  return (
    <JwtTokenContext.Provider
      value={{ jwtTokenState: [jwtToken, setJwtToken] }}
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
          <ChosenRestaurantIdContext.Provider
            value={{
              chosenRestaurantIdState: [
                chosenRestaurantId,
                setChosenRestaurantId,
              ],
            }}
          >
            <ChosenRestaurantDishesContext.Provider
              value={{
                chosenRestaurantDishesState: [
                  chosenRestaurantDishes,
                  setChosenRestaurantDishes,
                ],
              }}
            >
              <ChosenRestaurantTablesContext.Provider
                value={{
                  chosenRestaurantTablesState: [
                    chosenRestaurantTables,
                    setChosenRestaurantTables,
                  ],
                }}
              >
                <OrderContext.Provider
                  value={{ orderState: [order, setOrder] }}
                >
                  <BookingContext.Provider
                    value={{
                      bookingDateState: [bookingDate, setBookingDate],
                      bookingTableState: [bookingTable, setBookingTable],
                      bookingTimePeriodState: [
                        bookingTimePeriod,
                        setBookingTimePeriod,
                      ],
                    }}
                  >
                    {children}
                  </BookingContext.Provider>
                </OrderContext.Provider>
              </ChosenRestaurantTablesContext.Provider>
            </ChosenRestaurantDishesContext.Provider>
          </ChosenRestaurantIdContext.Provider>
        </DishCategoriesContext.Provider>
      </RestaurantsContext.Provider>
    </JwtTokenContext.Provider>
  );
}
