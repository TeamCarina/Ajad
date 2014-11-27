var RESULT_CLASS = 'kuler-finder-result',
	ITEM_CLASS = 'box-product list-layout',
	LOAD_MORE_CONTAINER_ID = 'load-more-container',
	LOAD_MORE_ID = 'load-more',
	LOADING_ID = 'kuler-finder-loading',
	LOADING_IMAGE_SRC = 'catalog/view/theme/'+ Kuler.theme +'/image/icon/loading.gif',
	LOAD_MORE_TEXT = Kuler.text_load_more;

jQuery(document).ready(function($) {
	var $kfInput = $('.kf_search'),
		$kfBtnSearch = $('.button-search'),
		$kfCategory = $('.kf_category'),
		$kfManufacturer = $('.kf_manufacturer'),
		$kfContainer = $kfInput.parent(),
		currentSearchUrl, responseData;

	function search() {
		var url = $('base').attr('href') + 'index.php?route=product/search';

		var search = $kfInput.val();

		if (search) {
			url += '&search=' + encodeURIComponent(search);
		}

		if ($kfCategory.length) {
			url  += '&category_id=' + $kfCategory.val();
		}

		window.location = url;
	}

	/* Search */
	$kfBtnSearch.bind('click', search);

	/* Press enter */
	$kfInput.bind('keydown', function(e) {
		if (e.keyCode == 13) {
			search();
		}
	});

	var uiCreate = $.ui.menu.prototype._create;
	$.ui.menu.prototype._create = function () {
		if (this.element.parents('.kuler-finder-content')) {
			this.element
				.addClass("ui-menu ui-widget ui-widget-content ui-corner-all")
				.attr({role:"listbox", "aria-activedescendant":"ui-active-menuitem"});
		} else {
			uiCreate.call(this);
		}
	};

	/* Autocomplete */
	var kulerFinder = $kfInput.autocomplete({
		delay: 0,
		appendTo: $kfContainer,
		source: function(request, response) {
			if($kfCategory.length) {
				request.category = $kfCategory.val();
			} else {
				request.category = '';
			}

			request.manufacturer_id = $kfManufacturer.length ? $kfManufacturer.val() : 0;

			currentSearchUrl = 'index.php?route=module/kuler_cp/liveSearch&filter_name=' +  encodeURIComponent(request.term) + '&filter_category_id=' + request.category + '&filter_manufacturer_id=' + request.manufacturer_id;

			$.ajax({
				url: currentSearchUrl,
				data: {},
				dataType: 'json',
				success: function(data) {
					responseData = data;

					if (!data.status) {
						return;
					}

					if (data.products.length == 0) {
						data.products.push({
							label: Kuler.text_no_results
						});
					}

					response($.map(data.products, function(item) {
						if(Object.keys(item).length < 3) {
							return item.label;
						} else {
							item.label = item.name;
							item.value = item.name;

							return item;
						}
					}));
				}
			});
		},
		select: function(event, ui) {
		},
		focus: function(event, ui) {
			return false;
		}
	})
		.data('autocomplete');

	kulerFinder._renderMenu = function (ul, item) {
		$.ui.autocomplete.prototype._renderMenu.apply(this, [ul, item]);

		// Click out & close the result
		ul.addClass(RESULT_CLASS);

		$.each([ul, $kfInput], function () {
			this.on('click.kf_force_open', function (evt) {
				evt.stopPropagation();
			});
		});

		$('body').on('click.kf_close_result', function (evt) {
			if (evt.originalEvent !== undefined) {
				$('.' + RESULT_CLASS).hide();
				$kfInput.val('');
			}
		});

		if (responseData.status && responseData.more) {
			// Init scroll
			ul
				.append('<li id="'+ LOAD_MORE_CONTAINER_ID +'"><a href="index.htm#load-more" id="'+ LOAD_MORE_ID +'" data-page="2">'+ LOAD_MORE_TEXT +'</a></li>');

			var $loadMoreContainer = $('#' + LOAD_MORE_CONTAINER_ID);

			$loadMoreContainer.find('a').on('click', function (evt) {
				evt.preventDefault();

				var $loadMore = $(this),
					page = $loadMore.data('page');

				// Show loading
				$loadMore
					.hide()
					.after('<img id="'+ LOADING_ID +'" src="'+ LOADING_IMAGE_SRC +'/index.htm-" />');

				// Load more products
				$.ajax({
					url: currentSearchUrl,
					data: {
						page: page
					},
					success: function (data) {
						$('#' + LOADING_ID).remove();

						if (!data.status) {
							return;
						}

						$.each(data.products, function (index, item) {
							$loadMoreContainer.before('<li class="'+ ITEM_CLASS +'">' + item.html + '</li>');
						});

						if (!data.more) {
							$('#' + LOAD_MORE_CONTAINER_ID).remove();
						} else {
							$loadMore
								.show()
								.data('page', page + 1);
						}
					},
					dataType: 'json'
				});
			});
		}
	};
	kulerFinder._renderItem = function( ul, item ) {

		if(Object.keys(item).length < 3) {
			return $.ui.autocomplete.prototype._renderItem.apply(this, [ul, item]);
		}

		var itemData = {
			label: item.label,
			value: item.value
		};

		return $('<li>')
			.addClass(ITEM_CLASS)
			.data("item.autocomplete", itemData)
			.append(item.html)
			.appendTo(ul);
	};
});