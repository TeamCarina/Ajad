jQuery(document).ready(function () {
	if (Kuler.image_zoom_type == 'outer_cloud' || Kuler.image_zoom_type == 'inner_cloud') {
		var zoom_params = {
			cursor: 'pointer',
			galleryActiveClass: 'active',
			imageCrossfade: true,
			lensShape: Kuler.lens_zoom_shape == 'rounded' ? 'round' : 'square'
		};

		if (Kuler.image_zoom_type == 'outer_cloud') {
			zoom_params.zoomWindowWidth = Kuler.zoom_window_width;
			zoom_params.zoomWindowHeight = Kuler.zoom_window_height;
		}

		if (Kuler.image_zoom_type == 'inner_cloud') {
			zoom_params.zoomType = 'inner';
		}

		$("#main-image").elevateZoom(zoom_params);
	}

	$('#image-additional a').on('click', function (e) {
		e.preventDefault();

		var imagePath = $(this).data('zoomImage');

		$('#main-image')
			.attr('src', imagePath)
			.attr('data-zoom-image', imagePath)
			.data('zoomImage', imagePath)
			.data('elevateZoom').swaptheimage(imagePath, imagePath);
	});

	if (Kuler.image_lightbox) {
		$("#main-image").bind("click", function(e) {
			var $this = $(this);

			var items = [{
				src: $this.data('zoomImage') || this.src
			}];

			$('.product-image-link').each(function () {
				if (items[0].src != this.href) {
					items.push({
						src: this.href
					});
				}
			});

			$.magnificPopup.open({
				items: items,
				gallery: {
					enabled: true
				},
				type: 'image',
				mainClass: 'mfp-fade'
			});

			e.preventDefault();
		});
	}

	$('.button-select-list .toggle').on('click', function (evt) {
		evt.preventDefault();

		var $this = $(this),
			checked = !$this.data('checked');

		$this.prev().prop('checked', checked);
		$this.data('checked', checked);
	});

	// Quantity
	$('.dynamic-number').each(function () {
		var $input = $(this),
			$dec = $($input.data('dec')),
			$inc = $($input.data('inc')),
			min = $input.data('min');

		$dec.on('click', function () {
			var val = parseInt($input.val());

			if (val > min) {
				$input.val(val - 1);
			}
		});

		$inc.on('click', function () {
			$input.val(parseInt($input.val()) + 1);
		});
	});
});