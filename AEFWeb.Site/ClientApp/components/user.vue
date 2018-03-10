<template>
    <div class="col-md-12">
        <div class="card">
            <div class="card-header" data-background-color="purple">
                <h4 class="title">Usuários</h4>
            </div>
            <div class="card-content">
                <router-link to="./userEdit" class="btn btn-primary">Novo</router-link>
                <div class="clearfix"></div>
            </div>
            <div class="card-content table-responsive">
                <table class="table">
                    <thead class="text-primary">
                        <tr>
                            <td>Nome</td>
                            <th>Email</th>
                            <th>Data de Nascimento</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="obj in users">
                            <td>{{obj.FirstName + " " + obj.LastName}}</td>
                            <td>{{obj.Email}}</td>
                            <td>{{obj.DateOfBirth}}</td>
                            <td>
                                <a href="#">
                                    <i class="material-icons">create</i>
                                </a>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <pagination :current-page="pagination.currentPage"
                            :total-pages="pagination.totalPages"
                            @page-changed="pageChanged">
                </pagination>
            </div>
        </div>
    </div>
</template>
<script>
    import { baseConfig } from '../base-config'
    import pagination from './pagination.vue'

    export default {
        data() {
            return {
                baseConfig,
                users: [],
                pagination: {
                    currentPage: baseConfig.currentPage,
                    totalPages: 0
                }
            }
        },
        methods: {
            getUsers: async function () {
                let response = await this.$http.get(baseConfig.baseUrl + '/api/User/get-all')
                this.users = response.Objects;
                this.pagination.totalPages = response.TotalPages;
            },
            pageChanged(pageNum) {
                this.pageOne.currentPage = pageNum
            }
        },
        async created() {
            this.getUsers();
        },
        components: {
            pagination
        }
    }
</script>
<style>
</style>
