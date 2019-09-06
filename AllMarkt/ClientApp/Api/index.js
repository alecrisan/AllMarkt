import Axios from "axios";

const categoriesBaseUrl = "/api/categories";
const privateMessagesBaseUrl = "/api/privateMessages";
const ordersBaseUrl = "/api/orders";
const productsBaseUrl = "/api/products";
const shopCommentsBaseUrl = "/api/shopComments";
const userBaseUrl = "/api/users";
const productCommentsBaseUrl = "/api/productComments";
const productCategoriesBaseUrl = "/api/productCategories";
const shopCategoryBaseUrl = "/api/shopCategory";

const Api = {
    categories: {
        async getAllAsync() {
            return await Api.helper.serverCaller(categoriesBaseUrl, null, Axios.get, true);
        },

        async addAsync(category) {
            return await Api.helper.serverCaller(categoriesBaseUrl, category, Axios.post, true);
        },

        async editAsync(category) {
            return await Api.helper.serverCaller(categoriesBaseUrl, category, Axios.put, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(categoriesBaseUrl + "/" + id, null, Axios.delete, true);
        }
    },

    shopComments: {
        async getAllAsync() {
            return await Api.helper.serverCaller(shopCommentsBaseUrl, null, Axios.get, false);
        },

        async getByShopIdAsync(id) {
            return await Api.helper.serverCaller(shopCommentsBaseUrl + "/" + id, null, Axios.get, false);
        },

        async addAsync(shopComment) {
            return await Api.helper.serverCaller(shopCommentsBaseUrl, shopComment, Axios.post, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(shopCommentsBaseUrl + "/" + id, null, Axios.delete, true);
        },

        async editAsync(shopComment) {
            return await Api.helper.serverCaller(shopCommentsBaseUrl, shopComment, Axios.put, true);
        }
    },

    orders: {
        async getAllAsync() {
            return await Api.helper.serverCaller(ordersBaseUrl, null, Axios.get, true);
        },
        async getShopOrdersAsync() {
            return await Api.helper.serverCaller(ordersBaseUrl + "/getShopOrders", null, Axios.get, true);
        },

        async getCustomerOrdersAsync() {
            return await Api.helper.serverCaller(ordersBaseUrl + "/getCustomerOrders", null, Axios.get, true);
        },

        async addAsync(order) {
            return await Api.helper.serverCaller(ordersBaseUrl, order, Axios.post, true);
        },

        async editAsync(order) {
            return await Api.helper.serverCaller(ordersBaseUrl, order, Axios.put, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(ordersBaseUrl + '/' + id, null, Axios.delete, true);
        }
    },

    privateMessages: {
        async getAllSentPrivateMessagesByUserAsync() {
            return await Api.helper.serverCaller(privateMessagesBaseUrl + "/asSender", null, Axios.get, true);
        },

        async getAllReceivedPrivateMessagesByUserAsync() {
            return await Api.helper.serverCaller(privateMessagesBaseUrl + "/asReceiver", null, Axios.get, true);
        },

        async getAllPrivateMessagesAsync() {
            return await Api.helper.serverCaller(privateMessagesBaseUrl, null, Axios.get, true);
        },

        async addPrivateMessageAsync(privateMessage) {
            return await Api.helper.serverCaller(privateMessagesBaseUrl, privateMessage, Axios.post, true);
        },

        async editPrivateMessageAsync(privateMessage) {
            return await Api.helper.serverCaller(privateMessagesBaseUrl + "/EditPrivateMessageAsync", privateMessage, Axios.put, true);
        },

        async deletePrivateMessage(privateMessageId) {
            return await Api.helper.serverCaller(privateMessagesBaseUrl + "/" + privateMessageId, null, Axios.delete, true);
        },

        async updateOrDeletePrivateMessage(privateMessage) {
            return await Api.helper.serverCaller(privateMessagesBaseUrl + "/UpdatePrivateMessageStatusAsync", privateMessage, Axios.put, true);
        }
    },

    productCategories: {
        async getAllAsync() {
            return await Api.helper.serverCaller(productCategoriesBaseUrl, null, Axios.get, true);
        },

        async getAllByShopAsync() {
            return await Api.helper.serverCaller(productCategoriesBaseUrl + "/shop", null, Axios.get, true);
        },

        async getAllBySelectedShopAsync(id) {
            return await Api.helper.serverCaller(productCategoriesBaseUrl + "/shop/" + id, null, Axios.get, true);
        },

        async addAsync(productCategory) {
            return await Api.helper.serverCaller(productCategoriesBaseUrl, productCategory, Axios.post, true);
        },

        async editAsync(productCategory) {
            return await Api.helper.serverCaller(productCategoriesBaseUrl, productCategory, Axios.put, true);
        },

        async deleteAsync(productCategoryId) {
            return await Api.helper.serverCaller(productCategoriesBaseUrl + '/' + productCategoryId, null, Axios.delete, true);
        }
    },

    products: {
        async getAllAsync() {
            return await Api.helper.serverCaller(productsBaseUrl, null, Axios.get, true);
        },

        async getAllByProductCategoryAsync(id) {
            return await Api.helper.serverCaller(productsBaseUrl + '/' + id, null, Axios.get, true);
        },

        async addAsync(product) {
            return await Api.helper.serverCaller(productsBaseUrl, product, Axios.post, true);
        },

        async editAsync(product) {
            return await Api.helper.serverCaller(productsBaseUrl, product, Axios.put, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(productsBaseUrl + '/' + id, null, Axios.delete, true);
        },

        async getProductsWithRatingByCategoryIdAsync(id) {
            return await Api.helper.serverCaller(productsBaseUrl+"/withRatingId="+id, null, Axios.get, true);
        },
        async getAllByShopAsync() {
            return await Api.helper.serverCaller(productsBaseUrl + "/shop", null, Axios.get, true);
        },
        async getAllBySelectedShopAsync(id) {
            return await Api.helper.serverCaller(productsBaseUrl + "/selectedShop=" + id, null, Axios.get, true);
        }
    },

    shops: {
        async getAllShopsAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/shops", null, Axios.get, true);
        },

        async getShopsByCategoryIdAsync(id) {
            return await Api.helper.serverCaller(userBaseUrl + "/shopCategoryId=" + id, null, Axios.get, true);
        },

        async getShopByUserIdAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/shops/byUser", null, Axios.get, true);
        },

        async getShopByIdAsync(id) {
            return await Axios.get(userBaseUrl + "/shops/byId" + "/" + id);
        },

        async editShopAsync(shop) {
            return await Api.helper.serverCaller(userBaseUrl + "/shops", shop, Axios.put, true);
        },

        async getMyDataAsShopAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/getMyDataAsShop", null, Axios.get, true);
        }
    },

    productComments: {
        async getAllAsync() {
            return await Api.helper.serverCaller(productCommentsBaseUrl, null, Axios.get, false);
        },

        async getByProductIdAsync(id) {
            return await Api.helper.serverCaller(productCommentsBaseUrl + "/" + id, null, Axios.get, false);
        },

        async addAsync(productComment) {
            return await Api.helper.serverCaller(productCommentsBaseUrl, productComment, Axios.post, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(productCommentsBaseUrl + "/" + id, null, Axios.delete, true);
        },

        async editAsync(productComment) {
            return await Api.helper.serverCaller(productCommentsBaseUrl, productComment, Axios.put, true);
        }
    },

    customers: {
        async getAllCustomersAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/customers", null, Axios.get, true);
        },

        async getCustomerByUserIdAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/customers/byUser", null, Axios.get, true);
        },

        async getCustomerByIdAsync(id) {
            return await Axios.get(userBaseUrl + "/customers/byId" + "/" + id);
        },

        async editCustomerAsync(customer) {
            return await Api.helper.serverCaller(userBaseUrl + "/customers", customer, Axios.put, true);
        },

        async getMyDataAsCustomerAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/getMyDataAsCustomer", null, Axios.get, true);
        }
    },

    users: {
        async getAllAsync() {
            return await Api.helper.serverCaller(userBaseUrl, null, Axios.get, true);
        },

        async getMyDataAsUserAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/getMyDataAsNormalUser", null, Axios.get, true);
        },

        async getByIdAsync() {
            return await Api.helper.serverCaller(userBaseUrl + "/byId", null, Axios.get, true);
        },

        async addAsync(user) {
            return await Api.helper.serverCaller(userBaseUrl, user, Axios.post, true);
        },

        async editAsync(user) {
            return await Api.helper.serverCaller(userBaseUrl, user, Axios.put, true);
        },

        async deleteAsync(id) {
            return await Api.helper.serverCaller(userBaseUrl + "/" + id, null, Axios.delete, true);
        },

        async registerAsync(user) {
            return await Api.helper.serverCaller(userBaseUrl + "/newUser", user, Axios.post, false);
        },

        async getUserRole() {
            return await Api.helper.serverCaller(userBaseUrl + "/getMyUserRole", null, Axios.get, true);
        },

        async disableUserByIdAsync(user) {
            return await Api.helper.serverCaller(userBaseUrl + "/disableUser", user, Axios.put, true);
        }
    },

    shopCategory: {
        async addShopCategoryLink(categoryId) {
            return await Api.helper.serverCaller(shopCategoryBaseUrl + '/' + categoryId, categoryId, Axios.post, true);
        },
        async deleteShopCategoryLink(categoryId) {
            return await Api.helper.serverCaller(shopCategoryBaseUrl + '/' + categoryId, null, Axios.delete, true);
        }
    },

    login: {
        async loginAsync(user) {
            return await Axios.post(userBaseUrl + "/login", user);
        }
    },

    helper: {
        async isTokenExpired(token) {
            var isExpired = false;
            await Axios.post(userBaseUrl + "/isTokenExpired", null,
                {
                    headers:
                    {
                        Authorization: 'Bearer ' + token,
                    }
                })
                .catch(function (error) {
                    if (error.response)
                        if (error.response.headers)
                            if (error.response.headers['token-expired'] === "true")
                                isExpired = true;
                });
            return isExpired;
        },

        getLocalToken() {
            var user = JSON.parse(localStorage.getItem("currentUser"));
            return user.token;
        },

        disconnectUser() {
            var anonymousUser = {
                token: null,
                email: null,
                displayName: null,
                userRole: null
            };
            localStorage.setItem("currentUser", JSON.stringify(anonymousUser));
        },

        async serverCaller(url, payload, functionCallback, requiresAuthorization) {

            var token = requiresAuthorization ? Api.helper.getLocalToken() : null;

            if (requiresAuthorization === true && token === null) {
                document.location.href = (document.location.href.split("#")[0] + "#/");
                location.reload();
            }
            else {
                var headers = {
                    headers:
                    {
                        Authorization: 'Bearer ' + token,
                    }
                };

                if (requiresAuthorization === true && await Api.helper.isTokenExpired(token) === true) {
                    Api.helper.disconnectUser();
                    document.location.href = (document.location.href.split("#")[0] + "#/");
                    location.reload();
                }
                else {
                    if (payload === null)
                        return await functionCallback(url, headers);
                    else
                        return await functionCallback(url, payload, headers);
                }
            }
        }
    }
}

export default Api