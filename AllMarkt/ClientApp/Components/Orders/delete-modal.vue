<template>
    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="deletetitle">Delete Order</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you want to delete this order?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" v-on:click="deleteAsync">Yes</button>
                    <button type="button" class="btn btn-secondary" v-on:click="hide">No</button>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import $ from "jquery";
    import Api from "../../Api";
    export default {
        data() {
            return {
                isLoading: false,
                order: {
                    id: 0
                }
            }
        },
        methods: {
            show(order) {
                this.order = order;
                $(this.$el).modal("show");
            },
            hide() {
                $(this.$el).modal("hide");
            },
            async deleteAsync() {
                try {
                    this.isLoading = true;
                    await Api.orders.deleteAsync(this.order.id);
                    this.hide();
                    this.$emit("submitted");
                } finally {
                    this.isLoading = false;
                }

            }


        }

    }
</script>