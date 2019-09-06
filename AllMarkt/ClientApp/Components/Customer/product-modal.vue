<template>
    <div class="modal fade bd-example-modal-xl" id="product-modal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-xl" role="document">
            <div style="height:900px ; overflow-y:scroll " class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title" id="product-header">{{product.name}}</h3>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="d-flex flex-row-reverse">
                        <div class="m-3 w-100 text-justify">
                            {{product.description}}
                        </div>
                        <div class="flex-shrink-1 border-1">
                            <img src="https://images-na.ssl-images-amazon.com/images/I/81mEIp4PMBL._SL1500_.jpg" style="width:100px; height:auto" />
                        </div>
                    </div>
                    <hr />
                    <div class="accordion" id="accordionContainer" role="tablist" aria-multiselectable="true">
                        <div class="card">

                            <div class="card-header" role="tab" id="headingOne1">
                                <a data-toggle="collapse" data-parent="#accordionContainer" data-target="#collapseOne1" aria-expanded="true"
                                   aria-controls="collapseOne1" v-on:click="changeArrowDirection">
                                    <div class="justify-content-between d-flex">
                                        <h5 class="mb-0">
                                            Reviews
                                            <star-rating v-bind:increment="0.1"
                                                         v-bind:star-size="20"
                                                         :read-only="true"
                                                         :show-rating="true"
                                                         v-model="product.averageRating">
                                            </star-rating>
                                        </h5>
                                        <h5 v-if="arrowDisplayDirection"> &#9660;</h5>
                                        <h5 v-else>&#9650;</h5>
                                    </div>
                                </a>
                            </div>

                            <div id="collapseOne1" class="collapse show" role="tabpanel" aria-labelledby="headingOne1" data-parent="#accordionContainer">

                                <div class="card-body">

                                    <div>
                                        <label>Rating:</label>
                                        <star-rating v-bind:increment="1"
                                                     v-bind:star-size="20"
                                                     :show-rating="false"
                                                     v-model="newComment.rating">
                                        </star-rating>
                                    </div>

                                    <div>
                                        <label>Comment:</label>
                                        <textarea class="form-control" rows="2" v-model="newComment.text" maxlength="1023"></textarea>
                                    </div>
                                    <div class="p-3 d-flex justify-content-end">
                                        <button class="btn btn-primary" v-on:click="addComment()">Add Comment</button>
                                    </div>

                                    <hr />
                                    <div id="please">
                                        <div class="accordion" id="accordionExample" v-for="review in reviewsToDisplay">
                                            <div class="card" id="card">
                                                <div class="card-header" id="headingOne" data-toggle="collapse" :data-target="'#collapse'+review.id" aria-expanded="false" :aria-controls="'#collapse'+review.id">
                                                    <div class="d-flex justify-content-between">
                                                        <div>
                                                            {{"Review Added By: "+review.addedByName}}
                                                        </div>
                                                        <div>
                                                            {{"Date Added: "+getDateFromString(review.dateSent)}}
                                                        </div>
                                                        <div class="d-flex flex-row">
                                                            <div>Rating&emsp;</div>

                                                            <star-rating v-bind:increment="0.1"
                                                                         v-bind:star-size="15"
                                                                         :read-only="true"
                                                                         :show-rating="false"
                                                                         v-model="review.rating">
                                                            </star-rating>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div :id="'collapse'+review.id" class="collapse" aria-labelledby="headingOne" data-parent="#accordionExample">
                                                    <div class="card-body">
                                                        {{review.text}}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div v-if="this.reviewsToDisplay.length !== this.reviews.length" class="d-flex justify-content-center">
                                            <button class="btn btn-link" v-on:click="showMore()">
                                                Show more...
                                            </button>
                                        </div>

                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>
<script>
    import $ from "jquery";
    import Api from "../../Api";
    import StarRating from 'vue-star-rating'


    export default {
        components: {
            StarRating
        },

        data() {
            return {
                nrOfReviewsToDisplay: 2,
                product: {
                    id: 0,
                    name: "",
                    description: "",
                    imageURI: "",
                    price: 0,
                    state: "",
                    averageRating: 0
                },
                reviewsToDisplay: [],
                reviews: [],
                newComment: {
                    rating: null,
                    text: "",
                    addedByName: "user",
                    addedById: 1,// TO ADD REAL USERS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    productId: 0,
                    productName: ""
                },
                arrowDisplayDirection: true
            }
        },
        methods: {
            getDateFromString(dateString) {
                var date = new Date(dateString);
                var result = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
                return result;
            },

            async show(product) {
                this.reviews = [];
                this.reviewsToDisplay = [];
                $(this.$el).modal("show");
                this.product = product;
                const result = await Api.productComments.getByProductIdAsync(this.product.id);
                this.reviews = result.data;
                this.reviewsToDisplay = this.reviews.slice(0, this.nrOfReviewsToDisplay);
            },
            showMore() {
                const l = this.reviewsToDisplay.length;
                if (l + this.nrOfReviewsToDisplay > this.reviews.length) {
                    this.reviewsToDisplay = this.reviews;
                }
                else {
                    this.reviewsToDisplay = this.reviews.slice(0, l + this.nrOfReviewsToDisplay);
                }
            },
            async addComment() {

                this.newComment.productId = this.product.id;
                this.newComment.productName = this.product.name;
                await Api.productComments.addAsync(this.newComment);
                const result = await Api.productComments.getByProductIdAsync(this.product.id);
                this.reviews = result.data;
                this.reviewsToDisplay = this.reviews.slice(0, this.nrOfReviewsToDisplay);
                this.newComment = {
                    rating: 0,
                    text: "",
                    addedByName: "user",
                    addedById: 3,// TO ADD REAL USERS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    productId: 0,
                    productName: ""
                }
            },
            changeArrowDirection() {
                this.arrowDisplayDirection = !this.arrowDisplayDirection;
            }
        }
    }
</script>