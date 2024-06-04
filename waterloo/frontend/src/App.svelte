<script lang="ts">
    type SearchHistory = {
        searchTerm: string;
        domain: string;
        searchEngine: string;
        matches: number;
        maxSearchCount: string;
    };

    type SearchResult = {
        success: boolean;
        errors: string[];
        rankings: SearchRanking[];
    };

    type SearchRanking = {
        url: string;
        position: number;
    };

    let searchHistory: SearchHistory[] = [];

    let error = "";
    let searchTerm = "";
    let domain = "";
    let searchEngine: string;
    let maxSearchCount: string;
    let searching = false;

    let searchTermInvalid: boolean | undefined;
    let domainInvalid: boolean | undefined;
    let searchEngineInvalid: boolean | undefined;
    let maxSearchCountInvalid: boolean | undefined;

    let results: SearchResult;
    async function search() {
        clearValidation();
        if (!validForm()) {
            return;
        }

        searching = true;
        results = await fetch(
            `http://localhost:5267/ranking?searchTerm=${searchTerm}&matchingDomain=${domain}&searchEngine=${searchEngine}&results=${maxSearchCount}`
        )
            .then(response => response.json())
            .catch(err => {
                error = err;
            });

        searching = false;
        if (!results.success) {
            error = results.errors.join(", ");
        } else {
            addToHistory(results.rankings.length);
        }
    }

    function addToHistory(matches: number) {
        searchHistory = [
            {
                searchTerm,
                domain,
                searchEngine,
                matches,
                maxSearchCount,
            },
            ...searchHistory,
        ];
    }

    function clearValidation() {
        searchTermInvalid = undefined;
        domainInvalid = undefined;
        searchEngineInvalid = undefined;
    }

    function validForm(): boolean {
        if (!searchTerm.trim()) {
            searchTermInvalid = true;
        }

        if (!domain.trim()) {
            domainInvalid = true;
        }

        if (!searchEngine) {
            searchEngineInvalid = true;
        }

        if (searchTermInvalid || domainInvalid || searchEngineInvalid) {
            return false;
        }

        return true;
    }

    async function repeatSearch(history: SearchHistory) {
        searchTerm = history.searchTerm;
        domain = history.domain;
        searchEngine = history.searchEngine;
        maxSearchCount = history.maxSearchCount;

        await search();
    }
</script>

<body>
    <header></header>
    <main class="container">
        <article>
            <header>
                Search
                <small>
                    - Provides the frequency and positions a site appears in the
                    results of a given search</small
                >
            </header>
            <form>
                <label>
                    Search Term
                    <input
                        type="text"
                        name="searchTerm"
                        placeholder="E.g. Land registry search"
                        required
                        bind:value={searchTerm}
                        aria-invalid={searchTermInvalid}
                        on:change={() => (searchTermInvalid = undefined)}
                    />
                </label>

                <label>
                    URL
                    <input
                        type="text"
                        name="matchDomain"
                        placeholder="E.g. www.infotrack.co.uk"
                        required
                        bind:value={domain}
                        aria-invalid={domainInvalid}
                        on:change={() => (domainInvalid = undefined)}
                    />
                </label>

                <div class="grid">
                    <label>
                        Search Engine
                        <select
                            name="searchEngine"
                            required
                            bind:value={searchEngine}
                            aria-invalid={searchEngineInvalid}
                            on:change={() => (searchEngineInvalid = undefined)}
                        >
                            <option>Google</option>
                        </select>
                    </label>

                    <label>
                        Maximum Search Count
                        <select
                            name="maxSearchCount"
                            required
                            bind:value={maxSearchCount}
                            aria-invalid={maxSearchCountInvalid}
                            on:change={() =>
                                (maxSearchCountInvalid = undefined)}
                        >
                            <option>10</option>
                            <option>50</option>
                            <option selected>100</option>
                            <option>200</option>
                        </select>
                    </label>
                </div>
                <button
                    type="submit"
                    on:click|preventDefault={search}
                    aria-busy={searching}>Search</button
                >
            </form>
        </article>

        <article>
            <header>Results</header>
            {#if searching}
                <progress />
            {/if}
            {#if results?.success}
                {#if results.rankings.length === 0}
                    <p>
                        No results found for this search. Did you include a
                        subdomain in the URL?
                    </p>
                {:else}
                    <p>{results.rankings.length} result(s) found!</p>
                    <div class="overflow-auto">
                        <table>
                            <thead>
                                <tr>
                                    <th scope="col">Position</th>
                                    <th scope="col">Link</th>
                                </tr>
                            </thead>
                            <tbody>
                                {#each results.rankings as ranking}
                                    <tr>
                                        <td>
                                            {ranking.position}
                                        </td>
                                        <td>
                                            <a
                                                href={ranking.url}
                                                target="_blank">{ranking.url}</a
                                            >
                                        </td>
                                    </tr>
                                {/each}
                            </tbody>
                        </table>
                    </div>
                {/if}
            {:else if error}
                <p class="error">{error}</p>
            {:else}
                <p>Please search using the above form</p>
            {/if}
        </article>

        {#if searchHistory.length > 0}
            <article>
                <header>Recent Searches</header>
                <div class="overflow-auto">
                    <table>
                        <thead>
                            <tr>
                                <th scope="col">Search Term</th>
                                <th scope="col">Matching Domain</th>
                                <th scope="col">Search Engine</th>
                                <th scope="col">Matches</th>
                            </tr>
                        </thead>
                        <tbody>
                            {#each searchHistory as history}
                                <tr
                                    class="cursor-pointer"
                                    on:click={() => repeatSearch(history)}
                                >
                                    <td>
                                        {history.searchTerm}
                                    </td>
                                    <td>
                                        {history.domain}
                                    </td>
                                    <td>
                                        {history.searchEngine}
                                    </td>
                                    <td>
                                        {history.matches}
                                    </td>
                                </tr>
                            {/each}
                        </tbody>
                    </table>
                </div>
            </article>
        {/if}
    </main>
</body>

<style>
    .cursor-pointer {
        cursor: pointer;
    }

    .error {
        color: red;
    }
</style>
